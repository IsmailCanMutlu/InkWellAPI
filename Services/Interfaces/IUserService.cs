using RestApiChallenge.Models.Requests;

namespace RestApiChallenge.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(UserRegisterRequest registerRequest);
    Task<string?> ValidateUserAsync(string username, string password);
}