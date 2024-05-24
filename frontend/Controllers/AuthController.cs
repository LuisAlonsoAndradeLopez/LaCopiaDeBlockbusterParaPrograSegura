using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class AuthController(AuthClientService auth) : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync(Login model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // This function verifies in the backend if the email and the password are valid.
                var token = await auth.ObtainTokenAsync(model.Email, model.Password);
                var claims = new List<Claim>
                {
                    // This is storaged in the cookie
                    new(ClaimTypes.Name, token.Email),
                    new(ClaimTypes.GivenName, token.Name),
                    new("jwt", token.Jwt),
                    new(ClaimTypes.Role, token.Role),
                };
                auth.LoginAsync(claims);

                // Valid user, is send to the Movies list
                return RedirectToAction("Index", "Peliculas");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "Credenciales no válidas. Intentelo nuevamente.");
            }
        }

        return View(model);
    }

    //[Authorize(Roles = "Administrador, Usuario")]
    public async Task<IActionResult> ExitAsync()
    {
        // Close the sesion
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Else, returns to the main menu
        return RedirectToAction("Index", "Auth");
    }
}