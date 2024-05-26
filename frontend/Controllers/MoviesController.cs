using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

//[Authorize(Roles = "Administrador, Usuario")]
public class MoviesController(MoviesClientService movies, CategoriesClientService categories) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Movie>? list = [];

        try
        {
            list = await movies.GetAsync(s);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        if (User.FindFirstValue(ClaimTypes.Role) == "Administrador")
            ViewBag.OnlyAdmin = true;

        ViewBag.search = s;
        return View(list);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Movie? item = null;
        try
        {
            item = await movies.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(item);
    }

    //[Authorize(Roles = "Administrador")]
    public IActionResult Make()
    {
        return View();
    }

    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> MakeAsync(Movie itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await movies.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        // Error case
        ModelState.AddModelError("Name", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(itemToCreate);
    }

    //[Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Movie? itemToEdit = null;
        try
        {
            itemToEdit = await movies.GetAsync(id);
            if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToEdit);
    }

    

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id, bool desition, bool? showError = false)
    {
        Movie? itemToDelete = null;
        try
        {
            if(desition)
            {
                await movies.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToDelete);
    }

    [AcceptVerbs("GET", "POST")]
    //[Authorize(Roles = "Administrador")]
    public IActionResult ValidatePoster(string Poster)
    {
        if (Uri.IsWellFormedUriString(Poster, UriKind.Absolute))
            return Json(true);

        return Json(false);
    }

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Categories(int id)
    {
        Movie? itemToView = null;
        try
        {
            itemToView = await movies.GetAsync(id);
            if (itemToView == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ViewData["MovieId"] = itemToView?.MovieId;
        return View(itemToView);
    }

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AddCategory(int id)
    {
        MovieCategory? itemToView = null;
        try
        {
            Movie? movie = await movies.GetAsync(id);
            if (movie == null) return NotFound();

            await CategoriesDropDownListAsync();
            itemToView = new MovieCategory { Movie = movie };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(itemToView);
    }

    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AddCategory(int id, int categoryid)
    {
        Movie? movie = null;

        if (ModelState.IsValid)
        {
            try
            {
                movie = await movies.GetAsync(id);
                if (movie == null) return NotFound();

                Category? category = await categories.GetAsync(categoryid);
                if (category == null) return NotFound();

                await movies.PostAsync(id, categoryid);
                return RedirectToAction(nameof(Categories), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        // Error case
        ModelState.AddModelError("id", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        await CategoriesDropDownListAsync();
        return View(new MovieCategory { Movie = movie });
    }

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> RemoveCategory(int id, int categoryid, bool? showError = false)
    {
        MovieCategory? itemToView = null;
        try
        {
            Movie? movie = await movies.GetAsync(id);
            if (movie == null) return NotFound();

            Category? category = await categories.GetAsync(categoryid);
            if (category == null) return NotFound();

            itemToView = new MovieCategory { Movie = movie, CategoryId = categoryid, Name = category.Name };
            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToView);
    }

    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> RemoveCategory(int id, int categoryid)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await movies.DeleteAsync(id, categoryid);
                return RedirectToAction(nameof(Categories), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        // Error case
        return RedirectToAction(nameof(RemoveCategory), new { id, categoryid, showError = true });
    }

    private async Task CategoriesDropDownListAsync(object? itemSeleccionado = null)
    {
        var listado = await categories.GetAsync();
        ViewBag.Category = new SelectList(listado, "CategoryId", "Name", itemSeleccionado);
    }
}