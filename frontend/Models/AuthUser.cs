namespace frontendnet.Models;

public class AuthUser
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Rol { get; set; }
    public required string Jwt { get; set; }
}