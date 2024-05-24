using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/movie")]
[ApiController]
public class MoviesController(IdentityContext context) : Controller
{

    // GET: api/movies?s=title
    [HttpGet]
    //[Authorize(Roles = "Usuario,Administrador")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return await context.Movie.Include(i => i.Categories).AsNoTracking().ToListAsync();

        return await context.Movie.Include(i => i.Categories).Where(c => c.Title.Contains(s)).AsNoTracking().ToListAsync();
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    //[Authorize(Roles = "Usuario,Administrador")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await context.Movie.Include(i => i.Categories).AsNoTracking().FirstOrDefaultAsync(s => s.MovieId == id);
        if (movie == null) return NotFound();

        return movie;
    }

    // POST: api/movies
    [HttpPost]
    //[Authorize(Roles = "Administrador")]
    public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
    {
        Movie movie = new()
        {
            Title = movieDTO.Title,
            Synopsis = movieDTO.Synopsis,
            Year = movieDTO.Year,
            Poster = movieDTO.Poster,
            Categories = []
        };

        context.Movie.Add(movie);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.MovieId }, movie);
    }

    // PUT: api/movies/5
    [HttpPut("{id}")]
    [Authorize]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
    {
        if (id != movieDTO.MovieId) return BadRequest();

        var movie = await context.Movie.FirstOrDefaultAsync(s => s.MovieId == id);
        if (movie == null) return NotFound();

        movie.Title = movieDTO.Title;
        movie.Synopsis = movieDTO.Synopsis;
        movie.Year = movieDTO.Year;
        movie.Poster = movieDTO.Poster;
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await context.Movie.FindAsync(id);
        if (movie == null) return NotFound();

        context.Movie.Remove(movie);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/movies/5/category
    [HttpPost("{id}/category")]
    //[Authorize(Roles = "Administrador")]
    public async Task<ActionResult> PostCategoryMovie(int id, AssignCategoryDTO itemToAdd)
    {
        Category? category = await context.Category.FindAsync(itemToAdd.CategoryId);
        if (category == null) return NotFound();

        var movie = await context.Movie.Include(i => i.Categories).FirstOrDefaultAsync(s => s.MovieId == id);
        if (movie == null) return NotFound();

        if (movie?.Categories?.FirstOrDefault(category) != null)
        {
            movie.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }

    // DELETE: api/movies/5/category/1
    [HttpDelete("{id}/category/{categoryid}")]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteCategoryMovie(int id, int categoryid)
    {
        Category? category = await context.Category.FindAsync(categoryid);
        if (category == null) return NotFound();

        var movie = await context.Movie.Include(i => i.Categories).FirstOrDefaultAsync(s => s.MovieId == id);
        if (movie == null) return NotFound();

        if (movie?.Categories?.FirstOrDefault(category) != null)
        {
            movie.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}