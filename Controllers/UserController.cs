using Microsoft.AspNetCore.Mvc;
using RestApiChallenge.Data;
using RestApiChallenge.Models;
using RestApiChallenge.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ApplicationDbContext _context;

    public UserController(UserService userService, ApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Kullanıcı kaydı işlemleri
        var userExists = await _context.Users.AnyAsync(u => u.Username == user.Username);
        if (userExists)
        {
            return BadRequest(new { message = "Username is already taken." });
        }

        var result = await _userService.RegisterUserAsync(user);
        if (result)
        {
            return Ok(new { message = "User registered successfully." });
        }

        return BadRequest(new { message = "User registration failed." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginDetails)
    {
        var token = await _userService.ValidateUserAsync(loginDetails.Username, loginDetails.Password);
        if (token != null)
        {
            return Ok(new { token = token });
        }

        return Unauthorized(new { message = "Username or password is incorrect." });
    }
}
