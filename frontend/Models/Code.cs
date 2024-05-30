using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Code
{
    [Required]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números.")]
    public string? Code_ { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} no es correo válido.")]
    [Display(Name = "Correo electrónico")]
    public required string Email { get; set; }
}