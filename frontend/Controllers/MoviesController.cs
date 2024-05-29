using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

//[Authorize(Roles = "Administrador, Usuario")]
public class MoviesController(MoviesClientService movies, CategoriesClientService categories) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Movie>? moviesList = [];

        try
        {
            moviesList = await movies.GetAsync(s);

            //List<string> imagesDataUrls = [];
//
            //foreach(Movie movie in moviesList)
            //{
            //    string base64Poster = Convert.ToBase64String(movie.Poster);
            //    string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
            //    imagesDataUrls.Add(imageDataUrl);
            //}
//
            //ViewBag.ImagesDataUrls = imagesDataUrls;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        if (User.FindFirstValue(ClaimTypes.Role) == "Administrador")
            ViewBag.OnlyAdmin = true;

        ViewBag.search = s;
        return View(moviesList);
    }

    //[Authorize(Roles = "Administrador")]
    public IActionResult Make()
    {
        return View();
    }

    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> MakeAsync(Movie itemToCreate, IFormFile Poster)
    {
        ModelState.Remove("Poster");

        if (Poster != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await Poster.CopyToAsync(memoryStream);
                itemToCreate.Poster = memoryStream.ToArray();
            }
        }
        else
        {
            itemToCreate.Poster = await new HttpClient().GetByteArrayAsync("https://via.placeholder.com/300x450");
        }

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

            if(itemToEdit.Poster != null) 
            {
                string base64Poster = Convert.ToBase64String(itemToEdit.Poster);
                string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
                ViewBag.ImageDataUrl = imageDataUrl;
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        return View(itemToEdit);
    }

    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EditAsync(int id, Movie itemToEdit, IFormFile Poster)
    {
        ModelState.Remove("Poster");

        if (id != itemToEdit.MovieId) return NotFound();        

        if (Poster != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await Poster.CopyToAsync(memoryStream);
                itemToEdit.Poster = memoryStream.ToArray();
            }
        }
        else
        {
            Movie itemToEditWithExistingImage = await movies.GetAsync(id);
            itemToEdit.Poster = itemToEditWithExistingImage?.Poster;
        }

        if (ModelState.IsValid)
        {
            try
            {
                await movies.PutAsync(itemToEdit);
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
        return View(itemToEdit);
    }

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id, bool? showError = false)
    {
        Movie? itemToDelete = null;
        try
        {
            itemToDelete = await movies.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            if(itemToDelete.Poster != null) 
            {
                string base64Poster = Convert.ToBase64String(itemToDelete.Poster);
                string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
                ViewBag.ImageDataUrl = imageDataUrl;
            }

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
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await movies.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        // En caso de error
        return RedirectToAction(nameof(Delete), new { id, showError = true });
    }

    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Categories(int id)
    {
        Movie? itemToView = null;
        try
        {
            itemToView = await movies.GetAsync(id);
            if (itemToView == null) return NotFound();

            if(itemToView.Poster != null) 
            {
                string base64Poster = Convert.ToBase64String(itemToView.Poster);
                string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
                ViewBag.ImageDataUrl = imageDataUrl;
            }
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

            if(movie.Poster != null) 
            {
                string base64Poster = Convert.ToBase64String(movie.Poster);
                string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
                ViewBag.ImageDataUrl = imageDataUrl;
            }
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
            Console.WriteLine(categoryid);

            Movie? movie = await movies.GetAsync(id);
            if (movie == null) return NotFound();

            Category? category = await categories.GetAsync(categoryid);
            if (category == null) return NotFound();

            itemToView = new MovieCategory { Movie = movie, CategoryId = categoryid, Name = category.Name };

            if(movie.Poster != null) 
            {
                string base64Poster = Convert.ToBase64String(movie.Poster);
                string imageDataUrl = $"data:image/jpeg;base64,{base64Poster}";
                ViewBag.ImageDataUrl = imageDataUrl;
            }

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