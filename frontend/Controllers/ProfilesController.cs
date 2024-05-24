using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize]
public class ProfileController(ProfileClientService profile) : Controller
{
    public async Task<IActionResult> IndexAsync()
    {
        AuthUser? user = null;
        try
        {
            ViewBag.timeRemaining = await profile.ObtainTimeAsync();

            // Obtiene el perfil del usuario
            user = new AuthUser
            {
                Email = User.FindFirstValue(ClaimTypes.Name)!,
                Name = User.FindFirstValue(ClaimTypes.GivenName)!,
                Role = User.FindFirstValue(ClaimTypes.Role)!,
                Jwt = User.FindFirstValue("jwt")!
            };            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                return RedirectToAction("Salir", "Auth");
        }

        return View(user);
    }
}