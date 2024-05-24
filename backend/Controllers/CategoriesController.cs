using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/categories")]
[ApiController]
//[Authorize(Roles = "Administrador")]
public class CategoriesController(IdentityContext context) : Controller
{

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await context.Category.AsNoTracking().ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await context.Category.FindAsync(id);
        if (category == null) return NotFound();

        return category;
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
    {
        Category category = new()
        {
            Name = categoryDTO.Name
        };

        context.Category.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.CategoryId) return BadRequest();

        var category = await context.Category.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = categoryDTO.Name;
        await context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await context.Category.FindAsync(id);
        if (category == null) return NotFound();

        if (category.IsProtected) return BadRequest();

        context.Category.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }
}