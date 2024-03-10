using Microsoft.AspNetCore.Mvc;
using RestApiChallenge.Models.Requests;
using RestApiChallenge.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequest)
    {
        var result = await _userService.RegisterUserAsync(registerRequest);
        if (result)
        {
            return Ok(new { message = "User registered successfully." });
        }

        return BadRequest(new { message = "User registration failed. Username might be already taken." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
    {
        var token = await _userService.ValidateUserAsync(loginRequest.Username, loginRequest.Password);
        if (token != null)
        {
            return Ok(new { token = token });
        }

        return Unauthorized(new { message = "Username or password is incorrect." });
    }
}