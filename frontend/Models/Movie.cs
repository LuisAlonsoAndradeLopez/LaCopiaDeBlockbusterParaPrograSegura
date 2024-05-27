using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public byte[]? Poster { get; set; }

    [Display(Name = "Eliminable")]
    public bool IsProtected { get; set; } = false;

    public ICollection<Category>? Categories { get; set; }
}

public class MovieWithByteArrayPoster
{
    public int? MovieId { get; set; }
    public string? Title { get; set; }
    public string? Synopsis { get; set; } 
    public int Year { get; set; }
    public byte[]? Poster { get; set; }  
    public ICollection<Category>? Categories { get; set; }
}