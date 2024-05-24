namespace backendnet.Models;

public class CustomIdentityUserPwdDTO
{
    public string? Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Role { get; set; }
}