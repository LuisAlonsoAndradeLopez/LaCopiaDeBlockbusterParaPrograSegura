using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class MovieCategory
{
    [Display (Name = "Categoría")]
    [Required (ErrorMessage = "El campo {0} es obligatorio.")]
    public int? CategoryId { get; set; }
    
    public string? Name { get; set; }

    public Movie? Movie { get; set; }
}