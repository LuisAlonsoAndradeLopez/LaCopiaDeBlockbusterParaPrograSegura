using System.Security.Claims;
using backendnet.Models;
using backendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backendnet.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(UserManager<CustomIdentityUser> userManager, JwtTokenService jwtTokenService) : Controller
{
    //POST api/auth
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] LoginDTO loginDTO)
    {
        //Verify credentials with Identity
        var user = await userManager.FindByEmailAsync(loginDTO.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            //Returns 401 Non autorized access
            return Unauthorized(new { message = "Usuario o contraseña inválidos." });
        }

        //These values indicates to the autenticated user in every request using the token
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.GivenName, user.Name),
            new(ClaimTypes.Role, roles.First())
        };

        //Create the access token
        var jwt = jwtTokenService.GenerateToken(claims);

        //Returns the access token to the user with the user, the token has 5 minutes lifetime
        return Ok(new
        {
            user.Email,
            user.Name,
            rol = string.Join(",", roles),
            jwt
        });
    }

    //GET: api/auth/tiempo
    [Authorize]
    [HttpGet("time")]
    public IActionResult GetTime()
    {
        string? time = jwtTokenService.TokenRemainingTime();
        if (time is null)
            return BadRequest();
        return Ok(time);
    }
}