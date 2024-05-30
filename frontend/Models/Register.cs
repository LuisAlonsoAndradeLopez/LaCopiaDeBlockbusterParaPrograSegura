using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Register
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} no es correo válido")]
    [Display(Name = "Correo electrónico")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Nombre")]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Nombre de usuario")]
    public required string NombreUsuario { get; set; }

}