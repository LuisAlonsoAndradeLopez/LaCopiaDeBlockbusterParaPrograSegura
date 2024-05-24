using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/roles")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class RolesController(IdentityContext context) : Controller
{
    //GET: api/roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRoleDTO>>> GetRoles()
    {
        var roles = new List<UserRoleDTO>();

        foreach (var rol in await context.Roles.AsNoTracking().ToListAsync())
        {
            roles.Add(new UserRoleDTO
            {
                Id = rol.Id,
                Name = rol.Name!
            });
        }

        return roles;
    }
}