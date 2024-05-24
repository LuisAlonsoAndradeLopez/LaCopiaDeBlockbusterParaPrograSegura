namespace backendnet.Models;

public class Movie
{
    //In this ORM, the key is the property with the word [class]Id
    public int MovieId { get; set; }
    public required string Title { get; set; } = "Without Title";
    public required string Synopsis { get; set; } = "Without Synopsis";
    public required int Year { get; set; }
    public byte[]? Poster { get; set; }

    public ICollection<Category>? Categories { get; set; }
}