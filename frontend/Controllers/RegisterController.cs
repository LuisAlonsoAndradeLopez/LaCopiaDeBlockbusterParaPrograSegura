using Microsoft.AspNetCore.Mvc;
using frontendnet.Models;
using frontendnet.Services;
using Newtonsoft.Json;

namespace frontendnet.Controllers
{
    public class RegisterController(UsersClientService usuarios, EmailClientService emailClientService) : Controller
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
                    bool result = await emailClientService.SendEmailAsync(itemToCreate.Email);
                    if (result)
                    {
                        TempData["Email"] = itemToCreate.Email;
                        TempData["Usuario"] = JsonConvert.SerializeObject(itemToCreate);
                        return RedirectToAction("VerifyEmail", "Register");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
                        return View(itemToCreate);
                    }
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
            ViewBag.Email = TempData["Email"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateCode(Code code)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isCodeValid = await emailClientService.VerifyCodeAsync(code.Email, code.Code_);
                    if (isCodeValid)
                    {
                        var usuarioJson = TempData["Usuario"]?.ToString();
                        if (usuarioJson != null)
                        {
                            var usuario = JsonConvert.DeserializeObject<UserPwd>(usuarioJson);
                            await usuarios.PostAsync(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("Codigo", "Error al recuperar datos del usuario.");
                    }
                    else
                    {
                        ModelState.AddModelError("Codigo", "Código de verificación incorrecto.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return RedirectToAction("Salir", "Auth");
                }
            }

            return View("VerifyEmail", code);  // Ensure to pass the model instance `code`
        }

    }
}