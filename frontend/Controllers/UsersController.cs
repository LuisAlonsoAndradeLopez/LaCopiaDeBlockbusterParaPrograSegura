using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

//[Authorize(Roles = "Administrador")]
public class UsersController(UsersClientService users, RolesClientService roles) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<User>? list = [];

        try
        {
            list = await users.GetAsync();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Salir", "Auth");
        }

        return View(list);
    }

    public async Task<IActionResult> Detail(string id)
    {
        User? item = null;

        try
        {
            item = await users.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Salir", "Auth");
        }

        return View(item);
    }

    public async Task<IActionResult> Make()
    {
        await RolesDropDownListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MakeAsync(UserPwd itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await users.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Email", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        await RolesDropDownListAsync();
        return View(itemToCreate);
    }

    [HttpGet("[controller]/[action]/{email}")]
    public async Task<IActionResult> EditAsync(string email)
    {
        User? itemToEdit = null;
        try
        {
            itemToEdit = await users.GetAsync(email);
            if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                return RedirectToAction("Salir", "Auth");
        }

        ViewBag.CanEdit = !(User.Identity?.Name == email);
        await RolesDropDownListAsync(itemToEdit?.Role);
        return View(itemToEdit);
    }

    [HttpPost("[controller]/[action]/{email}")]
    public async Task<IActionResult> EditAsync(string email, User itemToEdit)
    {
        if (email != itemToEdit.Email) return NotFound();
        if (ModelState.IsValid)
        {
            try 
            {
                await users.PutAsync(itemToEdit);
                return RedirectToAction(nameof (Index));
            }
            catch (HttpRequestException ex)
            {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Inténtelo nuevamente."); ViewBag.Can = !(User.Identity?.Name == email);
        await RolesDropDownListAsync(itemToEdit?.Role);
        return View(itemToEdit);
    }

    public async Task<IActionResult> Delete(string id, bool? showError = false)
    {
        User? itemToDelete = null;
        try
        {
            itemToDelete = await users.GetAsync(id);
                if (itemToDelete == null) return NotFound();
                if (showError.GetValueOrDefault())
                    ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ViewBag.CanEdit = !(User.Identity?.Name == id);
        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await users.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {   
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }
        
        return RedirectToAction(nameof(Delete), new { id, showError = true });
    }

    private async Task RolesDropDownListAsync(object ? selectedRole = null)
    {
        var list = await roles.GetAsync();
        ViewBag.Rol = new SelectList(list, "Nombre", "Nombre", selectedRole);
    }
}