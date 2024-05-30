using Microsoft.AspNetCore.Mvc;
using frontendnet.Models;
using frontendnet.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet.Controllers
{
    public class RegisterController (UsersClientService usuarios, EmailClientService emailClientService) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckMailAsync(UserPwd itemToCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //await emailClientService.SendEmailAsync(itemToCreate.Email);
                    //await usuarios.PostAsync(itemToCreate);
                    return RedirectToAction("VerifyEmail", "Register");
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return RedirectToAction("Salir", "Auth");
                }
            }
                ModelState.AddModelError("Email", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
                return View(itemToCreate);
        }

        
        public IActionResult VerifyEmail()
        {
            return View();
        }

    }
}