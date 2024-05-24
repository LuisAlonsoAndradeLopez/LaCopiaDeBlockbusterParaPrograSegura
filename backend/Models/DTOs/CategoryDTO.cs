namespace backendnet.Models;

public class CategoryDTO
{
    public int? CategoryId { get; set; }
    public required string Name { get; set; }
    public bool IsProtected { get; set; } = false;
}