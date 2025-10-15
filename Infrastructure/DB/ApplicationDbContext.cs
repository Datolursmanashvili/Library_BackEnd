using Domain.Entities.AuthorEntity;
using Domain.Entities.BookAuthorEntity;
using Domain.Entities.ProductEntity;
using Domain.Entities.RoleEntity;
using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var permission = new List<Permissions>();

        modelBuilder.Entity<User>().ToTable("AppUsers");
        modelBuilder.Entity<ApplicationRole>().ToTable("AspRoles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspUserRoles");

        modelBuilder.Ignore<IdentityUserLogin<string>>();
        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityUserClaim<string>>();
        modelBuilder.Ignore<IdentityRoleClaim<string>>();

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });
        modelBuilder.Entity<ApplicationRole>().HasData(
                   new ApplicationRole
                   {
                       Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                       Name = "RootAdmin",
                       NormalizedName = "ROOTADMIN",
                       CreatedAt = DateTime.MinValue,
                       UpdatedAt = DateTime.MinValue,
                       Permissions = permission,
                       ConcurrencyStamp = "11d16551-a780-477d-8b58-6091a7269cb3",
                   });


        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = "847aeaad-ec0e-4b85-b20e-1828fae116c9",
                Name = "user",
                NormalizedName = "USER",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
                Permissions = permission,
                ConcurrencyStamp = "5fb7f941-9d65-4354-96ef-e201532c2f1f",
                DeletedAt = DateTime.MinValue,
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "0f8fad5b-d9cb-429f-a165-70867528350e",
                SecurityStamp = "d14d334e-0893-403d-81d3-486d2700aafa",
                UserName = "SuperAdmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEL9PxquaUB30HvHfKoBvT5a6X/YrmS7efzjNH6b+AoFZTLYBGDxjsO8xKOUI5iz7fQ==",
                Email = "SuperAdmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                ConcurrencyStamp = "3bc03145-cc9e-4d45-9d5f-e38aa27a7a01",
                PNumber = "admin",
                FirstName = "admin",
                LastName = "admin",
                DepartmentId = 9,
            }
        );



        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
           new IdentityUserRole<string>
           {
               UserId = "0f8fad5b-d9cb-429f-a165-70867528350e", // ID пользователя
               RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210" // ID роли
           });
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

}
