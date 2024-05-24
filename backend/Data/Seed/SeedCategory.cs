using backendnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendnet.Data.Seed;

public class SeedCategory : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(
            new Category { CategoryId = 1, Name = "Acción", IsProtected = true },
            new Category { CategoryId = 2, Name = "Aventura", IsProtected = true },
            new Category { CategoryId = 3, Name = "Animación", IsProtected = true },
            new Category { CategoryId = 4, Name = "Ciencia ficción", IsProtected = true },
            new Category { CategoryId = 5, Name = "Comedia", IsProtected = true },
            new Category { CategoryId = 6, Name = "Crimen", IsProtected = true },
            new Category { CategoryId = 7, Name = "Documental", IsProtected = true },
            new Category { CategoryId = 8, Name = "Drama", IsProtected = true },
            new Category { CategoryId = 9, Name = "Familiar", IsProtected = true },
            new Category { CategoryId = 10, Name = "Fantasia", IsProtected = true },
            new Category { CategoryId = 11, Name = "Historia", IsProtected = true },
            new Category { CategoryId = 12, Name = "Horror", IsProtected = true },
            new Category { CategoryId = 13, Name = "Musica", IsProtected = true },
            new Category { CategoryId = 14, Name = "Misterio", IsProtected = true },
            new Category { CategoryId = 15, Name = "Romance", IsProtected = true }
        );
    }
}