using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IdentityContext context, UserManager<CustomIdentityUser> userManager) : Controller
{

    // GET: api/usuarios
    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomIdentityUserDTO>>> GetUsers()
    {
        var users = new List<CustomIdentityUserDTO>();

        foreach (var user in await context.Users.AsNoTracking().ToListAsync())
        {
            users.Add(new CustomIdentityUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Role = GetUserRole(user)
            });
        }

        return users;
    }

    // GET: api/usuarios/email
    [Authorize(Roles = "Administrador")]
    [HttpGet("{email}")]
    public async Task<ActionResult<CustomIdentityUserDTO>> GetUser(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null) return NotFound();

        return new CustomIdentityUserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email!,
            Role = GetUserRole(user)
        };
    }

    // POST: api/usuarios
    //[Authorize(Roles = "Usuario,Administrador")]
    [HttpPost]
    public async Task<ActionResult<CustomIdentityUserDTO>> PostUser(CustomIdentityUserPwdDTO userDTO)
    {
        var userToCreate = new CustomIdentityUser
        {
            UserName = userDTO.Email,
            Email = userDTO.Email,
            NormalizedEmail = userDTO.Email.ToUpper(),
            Name = userDTO.Name,
            NormalizedUserName = userDTO.Email.ToUpper()
        };

        //Add to the user
        IdentityResult result = await userManager.CreateAsync(userToCreate, userDTO.Password);        
        if (!result.Succeeded) return BadRequest(new { mensaje = "El usuario no se ha podido crear." });

        //Add the desire role
        result = await userManager.AddToRoleAsync(userToCreate, userDTO.Role);

        //Returns the created value
        var userViewModel = new CustomIdentityUserDTO
        {
            Id = userToCreate.Id,
            Name = userDTO.Name,
            Email = userDTO.Email,
            Role = userDTO.Role
        };

        return CreatedAtAction(nameof(GetUser), new { email = userDTO.Email }, userViewModel);
    }

    // PUT: api/usuarios/email
    [Authorize(Roles = "Administrador")]
    [HttpPut("{email}")]
    public async Task<IActionResult> PutUser(string email, CustomIdentityUserDTO userDTO)
    {
        if (email != userDTO.Email) return BadRequest();

        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        //Verify if the returned role exists
        if (await context.Roles.Where(r => r.Name == userDTO.Role).FirstOrDefaultAsync() == null) return NotFound();

        //Update the data
        user.Name = userDTO.Name;
        user.NormalizedUserName = userDTO.Email.ToUpper();
        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest();

        //Update the selected role
        foreach (var rol in await context.Roles.ToListAsync())
            await userManager.RemoveFromRoleAsync(user, rol.Name!);
        await userManager.AddToRoleAsync(user, userDTO.Role);

        return NoContent();
    }

    // DELETE: api/usuarios/email
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        if (user.IsProtected) return StatusCode(StatusCodes.Status403Forbidden);

        IdentityResult result = await userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest();

        return NoContent();
    }

    private string GetUserRole(CustomIdentityUser user)
    {
        var roles = userManager.GetRolesAsync(user).Result;
        return roles.FirstOrDefault();
    }
}