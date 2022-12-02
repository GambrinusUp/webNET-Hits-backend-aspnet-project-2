using FoodDelivery.Models;
using FoodDelivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodDelivery.Services;
using FoodDelivery.Models.DTO;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
    {
        if (ModelState.IsValid)
        {
            if (_usersService.IsUserUnique(model))
            {
                await _usersService.Register(model);
                return new JsonResult(new
                {
                    token = _usersService.GetToken(
                    ConverterDTO.Login(model))
                });
            }
            else
            {
                return Conflict(new { errorText = "User already exists" });
            }
        }
        return BadRequest(new { errorText = "Registeration failed" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginCredentials userLoginData)
    {
        if (ModelState.IsValid)
        {
            var jwt = _usersService.GetToken(userLoginData);
            if (jwt != null)
            {
                return new JsonResult(new { token = jwt });
            }
            return Unauthorized(new { errorText = "Wrong email or password" });
        }
        return BadRequest(new { errorText = "Login failed" });
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
        //if (_logoutService.IsUserLoggedOut(token))
          //  return Unauthorized();

        try
        {
            var user = _usersService.GetUser(token);
            if (user == null)
                return BadRequest(new { errorText = "User not found" });

            return Ok(user);
        }
        catch
        {
            return BadRequest(new { errorText = "Internal error" });
        }
    }

    [HttpPut("profile")]
    public IActionResult EditProfile([FromBody] UserEditDTO model)
    {
        //try catch for null token
        string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
        //if (_logoutService.IsUserLoggedOut(token))
          //  return Unauthorized();

        if (ModelState.IsValid)
        {
            try
            {
                if (_usersService.EditUser(model, token))
                {
                    return Ok();
                }
            }
            catch { return BadRequest(new { errorText = "Failed to change user data." }); }
        }
        return BadRequest(new { errorText = "Invalid data." });

    }
}
