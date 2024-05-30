using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace frontendnet.Models;

public class Movie
{
    [Display(Name = "Id")]
    public int? MovieId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Título")]
     public required string Title { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Sinópsis")]
    public string Synopsis { get; set; } = "Sin sinopsis";

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1950, 2024, ErrorMessage = "El valor del campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Año")]
    public int Year { get; set; }

    public byte[]? Poster { get; set; }

    [Display(Name = "Eliminable")]
    public bool IsProtected { get; set; } = false;

    public ICollection<Category>? Categories { get; set; }
}


public class FileExtensionAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public FileExtensionAttribute(params string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var fileBytes = value as byte[];
        if (fileBytes == null || fileBytes.Length == 0)
        {
            return new ValidationResult("El archivo no es válido.");
        }

        var fileExtension = Path.GetExtension(validationContext.MemberName ?? "").ToLowerInvariant();
        if (!_extensions.Contains(fileExtension))
        {
            return new ValidationResult($"El archivo debe ser de tipo: {string.Join(", ", _extensions)}");
        }

        return ValidationResult.Success;
    }
}

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxFileSize;

    public MaxFileSizeAttribute(long maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var fileBytes = value as byte[];
        if (fileBytes == null || fileBytes.Length == 0)
        {
            return new ValidationResult("El archivo no es válido.");
        }

        if (fileBytes.Length > _maxFileSize)
        {
            return new ValidationResult($"El tamaño del archivo no puede ser mayor a {_maxFileSize / 1024 / 1024} MB.");
        }

        return ValidationResult.Success;
    }
}