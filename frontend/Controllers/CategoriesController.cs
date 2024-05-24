using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

//[Authorize(Roles = "Administrador")]
public class CategoriesController(CategoriesClientService categories) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Category>? list = [];
        try
        {
            list = await categories.GetAsync();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Salir", "Auth");
        }

        return View(list);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Category? item = null;

        try
        {
            item = await categories.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(item);
    }

    public IActionResult Make()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> MakeAsync(Category itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await categories.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(itemToCreate);
    }

    public async Task<IActionResult> EditAsync(int id)
    {
        Category? itemToEdit = null;

        try
        {
            itemToEdit = await categories.GetAsync(id); if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> EditAsync(int id, Category itemToEdit)
    {
        if (id != itemToEdit.CategoryId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await categories.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(itemToEdit);
    }

    public async Task<IActionResult> Delete(int id, bool? showError = false)
    {
        Category? itemToDelete = null;
        try
        {
            itemToDelete = await categories.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await categories.DeleteAsync(id);
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
}