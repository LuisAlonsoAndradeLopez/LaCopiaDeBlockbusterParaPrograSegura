using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Category
{
    [Display(Name = "Id")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")] 
    public required string Name { get; set; }
    
    [Display(Name = "Eliminable")]
    public bool IsProtected { get; set; } = false;
}