using RestApiChallenge.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestApiChallenge.Services{
public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKeyValue = jwtSettings.GetSection("SecretKey").Value;
if (string.IsNullOrEmpty(secretKeyValue))
{
    throw new InvalidOperationException("JWT Secret Key must be set in the configuration.");
}
var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));

        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings.GetSection("Issuer").Value,
            audience: jwtSettings.GetSection("Audience").Value,
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }
}
}