namespace RestApiChallenge.Services.Interfaces;

public interface IAuthService
{
    string GenerateToken(int userId, string username);
}