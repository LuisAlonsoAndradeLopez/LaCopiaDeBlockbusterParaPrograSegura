using Microsoft.AspNetCore.Mvc;
using frontendnet.Models;

namespace frontendnet;

public class RegisterController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CheckMail(Register model)
    {
        if (ModelState.IsValid)
        {
            // Aquí podrías realizar la lógica de envío de correo y generación de código.
            return RedirectToAction("VerifyEmail", "Register");
        }
        return View("Index", model);
    }

    public IActionResult VerifyEmail()
    {
        return View();
    }

}