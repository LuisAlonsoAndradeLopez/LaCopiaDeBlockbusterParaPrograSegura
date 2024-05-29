using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class RegisterController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult VerifyEmail()
    {
        return View();
    }

}