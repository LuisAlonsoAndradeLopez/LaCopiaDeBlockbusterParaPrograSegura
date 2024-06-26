using Microsoft.AspNetCore.Identity;

namespace backendnet.Models;

public class CustomIdentityUser : IdentityUser
{
    public required string Name { get; set; }
    public bool IsProtected { get; set; } = false;
}