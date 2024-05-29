using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace frontendnet.Models;

public class Movie
{
    [Display(Name = "Id")]
    public int? MovieId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
     public required string Title { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.MultilineText)]
    public string Synopsis { get; set; } = "Sin sinopsis";

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1950, 2024, ErrorMessage = "El valor del campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Año")]
    public int Year { get; set; }

    //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [MaxLength(1048576, ErrorMessage = "El tamaño del archivo no puede ser mayor a 1 MB.")]
    public byte[]? Poster { get; set; }

    [Display(Name = "Eliminable")]
    public bool IsProtected { get; set; } = false;

    public ICollection<Category>? Categories { get; set; }
}
