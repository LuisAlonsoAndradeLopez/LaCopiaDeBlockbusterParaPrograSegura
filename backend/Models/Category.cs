using System.Text.Json.Serialization;

namespace backendnet.Models;

public class Category
{
    //In this ORM, the key is the property with the word [class]Id
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public bool IsProtected { get; set; } = false;

    [JsonIgnore]
    public ICollection<Movie>? Movies { get; set; }
}