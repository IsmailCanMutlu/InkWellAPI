using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using RestApiChallenge.Data;
using RestApiChallenge.Models;
using System.Security.Claims;


namespace RestApiChallenge.Services{
public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApplicationDbContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            // Kullanıcı zaten var
            return false;
        }

        // Şifreyi hash'le
        user.Password = _passwordHasher.HashPassword(user, user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string?> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success)
        {
            // Kullanıcı doğrulandı, JWT token üret
            return GenerateJwtToken(user);
        }

        // Kullanıcı doğrulanamadı
        return null;
    }

    private string GenerateJwtToken(User user)
    {
       var secretKeyConfig = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKeyConfig))
        {
            throw new InvalidOperationException("JWT Secret Key must be set in the configuration.");
        }

        var key = Encoding.UTF8.GetBytes(secretKeyConfig); 
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
}
