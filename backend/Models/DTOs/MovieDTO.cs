namespace backendnet.Models;

public class MovieDTO
{
    public int? MovieId { get; set; }
    public required string Title { get; set; }
    public string Synopsis { get; set; } = "Sin sinopsis";
    public int Year { get; set; }
    public byte[]? Poster { get; set; }  
    public int[]? Categories { get; set; }
}