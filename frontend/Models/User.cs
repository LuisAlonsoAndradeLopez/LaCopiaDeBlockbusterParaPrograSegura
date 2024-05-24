using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class User
{
    [Display(Name = "Id")]
    public string? Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} no es correo válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")] 
    public required string Role { get; set; }
}