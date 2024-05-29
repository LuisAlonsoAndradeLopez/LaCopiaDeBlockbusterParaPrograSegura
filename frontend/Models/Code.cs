using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Code
{
    [Required]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números.")]
    public string? Code_ { get; set; }
}