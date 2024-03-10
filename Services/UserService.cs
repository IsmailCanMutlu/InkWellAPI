using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestApiChallenge.Data;
using RestApiChallenge.Models;
using RestApiChallenge.Models.Requests;
using RestApiChallenge.Services.Interfaces;


namespace RestApiChallenge.Services;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApplicationDbContext context, IConfiguration configuration, IAuthService authService,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _authService = authService;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> RegisterUserAsync(UserRegisterRequest registerRequest)
    {
        var user = new User
        {
            Username = registerRequest.Username,
            Password = registerRequest.Password,
            Name = registerRequest.Name,
            Surname = registerRequest.Surname
        };
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            // User already exists
            return false;
        }

        // Hash the password
        user.Password = _passwordHasher.HashPassword(user, user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string?> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, password) ==
            PasswordVerificationResult.Success)
        {
            // User verified, generate JWT token
            return _authService.GenerateToken(user.Id, user.Username);
        }
        
        return null;
    }
}