using backendnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Data.Seed;

public static class SeedIdentityUserData
{
    public static void SeedUserIdentityData(this ModelBuilder modelBuilder)
    {
        //Adding the role "Administrador" to the "AspNetRoles" table
        string AdministradorRoleId = Guid.NewGuid().ToString();        
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = AdministradorRoleId,
            Name = "Administrador",
            NormalizedName = "Administrador".ToUpper()
        });
//
        //Adding the role "Usuario" to the "AspNetRoles" table
        string UsuarioRoleId = Guid.NewGuid().ToString();
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = UsuarioRoleId,
            Name = "Usuario",
            NormalizedName = "Usuario".ToUpper()
        });

        
        var UserId = Guid.NewGuid().ToString();
        modelBuilder.Entity<CustomIdentityUser>().HasData
        (
            new CustomIdentityUser
            {
                Id = UserId, //primary key
                UserName = "caffeinated340@gmail.com",
                Email = "caffeinated340@gmail.com",
                NormalizedEmail = "caffeinated340@gmail.com".ToUpper(),
                Name = "Administrador",
                NormalizedUserName = "caffeinated340@gmail.com".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "LaContraseñaIndescriptible#234"),
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
                UserName = "caffeinated333@gmail.com",
                Email = "caffeinated333@gmail.com",
                NormalizedEmail = "caffeinated333@gmail.com".ToUpper(),
                Name = "Usuario",
                NormalizedUserName = "caffeinated333@gmail.com".ToUpper(),
                PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null!, "Usuario#234")
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
    }
}