namespace backendnet.Models;

public class CustomIdentityUserDTO
{
    public string? Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Role { get; set; }
}