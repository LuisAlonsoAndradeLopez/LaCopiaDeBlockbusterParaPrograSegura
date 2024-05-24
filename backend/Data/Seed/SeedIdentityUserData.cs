using backendnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Data.Seed;

public static class SeedIdentityUserData
{
    public static void SeedUserIdentityData(this ModelBuilder modelBuilder)
    {
        //Adding the role "Administrador" to the "AspNetRoles" table
        string AdminRoleId = Guid.NewGuid().ToString();        
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = AdminRoleId,
            Name = "Administrador",
            NormalizedName = "Administrador".ToUpper()
        });

        //Adding the role "Usuario" to the "AspNetRoles" table
        string UserRoleId = Guid.NewGuid().ToString();
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = UserRoleId,
            Name = "Usuario",
            NormalizedName = "Usuario".ToUpper()
        });

        /*
        var UserId = Guid.NewGuid().ToString();
        modelBuilder.Entity<CustomIdentityUser>().HasData
        (
            new CustomIdentityUser
            {
                Id = UserId, //primary key
                UserName = "gvera@uv.mx",
                Email = "gvera@uv.mx",
                NormalizedEmail = "gvera@uv.mx".ToUpper(),
                Name = "Guillermo Humberto Vera Amaro",
                NormalizedUserName = "gvera@uv.mx".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "patito"),
                IsProtected = true //Este no se puede eliminar
            }
        );

        //Aplicamos la relación entre el usuario y el rol en la tabla AspNetUserRoles
        modelBuilder.Entity<IdentityUserRole<string>>().HasData
        (
            new IdentityUserRole<string>
            {
                RoleId = AdministradorRoleId,
                UserId = UserId
            }
        );

        //Agregamos un usuario a la tabla AspNetUser
        UserId = Guid.NewGuid().ToString();
        modelBuilder.Entity<CustomIdentityUser>().HasData
        (
            new CustomIdentityUser
            {
                Id = UserId, //primary key
                UserName = "patito@uv.mx",
                Email = "patito@uv.mx",
                NormalizedEmail = "patito@uv.mx".ToUpper(),
                Name = "Usuario patito",
                NormalizedUserName = "patito@uv.mx".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "patito")
            }
        );

        //Aplicamos la relación entre el usuario y el rol en la tabla AspNetUserRoles
        modelBuilder.Entity<IdentityUserRole<string>>().HasData
        (
            new IdentityUserRole<string>
            {
                RoleId = UsuarioRoleId,
                UserId = UserId
            }
        );
        */
    }
}