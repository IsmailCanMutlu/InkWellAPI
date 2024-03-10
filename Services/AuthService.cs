using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestApiChallenge.Services.Interfaces;
using RestApiChallenge.Settings;

namespace RestApiChallenge.Services;

public sealed class AuthService : IAuthService
{
    private readonly JwtSettings _options;

    public AuthService(IOptions<JwtSettings> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(int userId, string username)
    {
        var key = Encoding.ASCII.GetBytes(_options.IssuerSigningKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public static class AuthExtensions
{
    public static string GetPrincipal(this ClaimsPrincipal claimsPrincipal, string claimValueTypes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(claimValueTypes);
        return claimsPrincipal.FindFirstValue(claimValueTypes);
    }
}