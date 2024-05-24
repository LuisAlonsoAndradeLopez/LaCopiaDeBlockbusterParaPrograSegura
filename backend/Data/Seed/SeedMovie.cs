using backendnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendnet.Data.Seed;

public class SeedMovie : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasData(
            new Movie { MovieId = 1, Title = "Pichardo la Película", Synopsis = "El banquero Andy Dufresne es arrestado por matar a su esposa y amante. Tras una dura adaptación, intenta mejorar las condiciones de la prisión y dar esperanza a sus compañeros.", Year = 1994 },
            new Movie { MovieId = 2, Title = "Chucky 2", Synopsis = "El patriarca de una organización criminal transfiere el control de su clandestino imperio a su reacio hijo.", Year = 1972 },
            new Movie { MovieId = 3, Title = "Fue - Bebé la Película", Synopsis = "Cuando el Joker emerge causando caos y violencia en Gotham, el caballero de la noches deberá aceptar una de las pruebas psicológicas y físicas más difíciles para poder luchar con las injusticias del enemigo.", Year = 2008 }
        );

        // Agrega las categorias a cada película
        builder.HasMany(c => c.Categories).WithMany(c => c.Movies).UsingEntity(j => j.HasData(
            new { PeliculasPeliculaId = 1, CategoriasCategoriaId = 6 },
            new { PeliculasPeliculaId = 2, CategoriasCategoriaId = 6 },
            new { PeliculasPeliculaId = 2, CategoriasCategoriaId = 8 },
            new { PeliculasPeliculaId = 3, CategoriasCategoriaId = 1 },
            new { PeliculasPeliculaId = 3, CategoriasCategoriaId = 2 },
            new { PeliculasPeliculaId = 3, CategoriasCategoriaId = 8 }
        ));
    }
}