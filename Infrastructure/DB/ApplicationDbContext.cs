using Domain.Entities.AuthorEntity;
using Domain.Entities.BookAuthorEntity;
using Domain.Entities.LocationEntity;
using Domain.Entities.ProductEntity;
using Domain.Entities.PublisherEntity;
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
               UserId = "0f8fad5b-d9cb-429f-a165-70867528350e", 
               RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210" 
           });



        var locations = new List<Location>
{
    // Georgia
    new Location { Id = 1, Name = "Georgia", IsCountry = true, ParentId = null },
    new Location { Id = 2, Name = "Tbilisi", IsCountry = false, ParentId = 1 },
    new Location { Id = 3, Name = "Batumi", IsCountry = false, ParentId = 1 },
    new Location { Id = 4, Name = "Kutaisi", IsCountry = false, ParentId = 1 },
    new Location { Id = 5, Name = "Rustavi", IsCountry = false, ParentId = 1 },
    new Location { Id = 6, Name = "Gori", IsCountry = false, ParentId = 1 },
    new Location { Id = 7, Name = "Zugdidi", IsCountry = false, ParentId = 1 },
    new Location { Id = 8, Name = "Poti", IsCountry = false, ParentId = 1 },
    new Location { Id = 9, Name = "Kobuleti", IsCountry = false, ParentId = 1 },
    new Location { Id = 10, Name = "Khashuri", IsCountry = false, ParentId = 1 },
    new Location { Id = 11, Name = "Senaki", IsCountry = false, ParentId = 1 },
    
    // Germany
    new Location { Id = 12, Name = "Germany", IsCountry = true, ParentId = null },
    new Location { Id = 13, Name = "Berlin", IsCountry = false, ParentId = 12 },
    new Location { Id = 14, Name = "Munich", IsCountry = false, ParentId = 12 },
    new Location { Id = 15, Name = "Hamburg", IsCountry = false, ParentId = 12 },
    new Location { Id = 16, Name = "Frankfurt", IsCountry = false, ParentId = 12 },
    new Location { Id = 17, Name = "Cologne", IsCountry = false, ParentId = 12 },
    new Location { Id = 18, Name = "Stuttgart", IsCountry = false, ParentId = 12 },
    new Location { Id = 19, Name = "Dusseldorf", IsCountry = false, ParentId = 12 },
    new Location { Id = 20, Name = "Dortmund", IsCountry = false, ParentId = 12 },
    new Location { Id = 21, Name = "Essen", IsCountry = false, ParentId = 12 },
    new Location { Id = 22, Name = "Leipzig", IsCountry = false, ParentId = 12 },
    new Location { Id = 23, Name = "Bremen", IsCountry = false, ParentId = 12 },
    new Location { Id = 24, Name = "Dresden", IsCountry = false, ParentId = 12 },
    new Location { Id = 25, Name = "Hanover", IsCountry = false, ParentId = 12 },
    new Location { Id = 26, Name = "Nuremberg", IsCountry = false, ParentId = 12 },
    
    // United States
    new Location { Id = 27, Name = "United States", IsCountry = true, ParentId = null },
    new Location { Id = 28, Name = "New York", IsCountry = false, ParentId = 27 },
    new Location { Id = 29, Name = "Los Angeles", IsCountry = false, ParentId = 27 },
    new Location { Id = 30, Name = "Chicago", IsCountry = false, ParentId = 27 },
    new Location { Id = 31, Name = "Houston", IsCountry = false, ParentId = 27 },
    new Location { Id = 32, Name = "Phoenix", IsCountry = false, ParentId = 27 },
    new Location { Id = 33, Name = "Philadelphia", IsCountry = false, ParentId = 27 },
    new Location { Id = 34, Name = "San Antonio", IsCountry = false, ParentId = 27 },
    new Location { Id = 35, Name = "San Diego", IsCountry = false, ParentId = 27 },
    new Location { Id = 36, Name = "Dallas", IsCountry = false, ParentId = 27 },
    new Location { Id = 37, Name = "San Jose", IsCountry = false, ParentId = 27 },
    new Location { Id = 38, Name = "Austin", IsCountry = false, ParentId = 27 },
    new Location { Id = 39, Name = "Jacksonville", IsCountry = false, ParentId = 27 },
    new Location { Id = 40, Name = "Fort Worth", IsCountry = false, ParentId = 27 },
    new Location { Id = 41, Name = "Columbus", IsCountry = false, ParentId = 27 },
    new Location { Id = 42, Name = "San Francisco", IsCountry = false, ParentId = 27 },
    new Location { Id = 43, Name = "Charlotte", IsCountry = false, ParentId = 27 },
    new Location { Id = 44, Name = "Indianapolis", IsCountry = false, ParentId = 27 },
    new Location { Id = 45, Name = "Seattle", IsCountry = false, ParentId = 27 },
    new Location { Id = 46, Name = "Denver", IsCountry = false, ParentId = 27 },
    new Location { Id = 47, Name = "Boston", IsCountry = false, ParentId = 27 },
    new Location { Id = 48, Name = "Nashville", IsCountry = false, ParentId = 27 },
    new Location { Id = 49, Name = "Detroit", IsCountry = false, ParentId = 27 },
    new Location { Id = 50, Name = "Portland", IsCountry = false, ParentId = 27 },
    new Location { Id = 51, Name = "Las Vegas", IsCountry = false, ParentId = 27 },
    new Location { Id = 52, Name = "Miami", IsCountry = false, ParentId = 27 },
    
    // France
    new Location { Id = 53, Name = "France", IsCountry = true, ParentId = null },
    new Location { Id = 54, Name = "Paris", IsCountry = false, ParentId = 53 },
    new Location { Id = 55, Name = "Marseille", IsCountry = false, ParentId = 53 },
    new Location { Id = 56, Name = "Lyon", IsCountry = false, ParentId = 53 },
    new Location { Id = 57, Name = "Toulouse", IsCountry = false, ParentId = 53 },
    new Location { Id = 58, Name = "Nice", IsCountry = false, ParentId = 53 },
    new Location { Id = 59, Name = "Nantes", IsCountry = false, ParentId = 53 },
    new Location { Id = 60, Name = "Strasbourg", IsCountry = false, ParentId = 53 },
    new Location { Id = 61, Name = "Montpellier", IsCountry = false, ParentId = 53 },
    new Location { Id = 62, Name = "Bordeaux", IsCountry = false, ParentId = 53 },
    new Location { Id = 63, Name = "Lille", IsCountry = false, ParentId = 53 },
    new Location { Id = 64, Name = "Rennes", IsCountry = false, ParentId = 53 },
    new Location { Id = 65, Name = "Reims", IsCountry = false, ParentId = 53 },
    new Location { Id = 66, Name = "Saint-Etienne", IsCountry = false, ParentId = 53 },
    new Location { Id = 67, Name = "Toulon", IsCountry = false, ParentId = 53 },
    
    // Italy
    new Location { Id = 68, Name = "Italy", IsCountry = true, ParentId = null },
    new Location { Id = 69, Name = "Rome", IsCountry = false, ParentId = 68 },
    new Location { Id = 70, Name = "Milan", IsCountry = false, ParentId = 68 },
    new Location { Id = 71, Name = "Naples", IsCountry = false, ParentId = 68 },
    new Location { Id = 72, Name = "Turin", IsCountry = false, ParentId = 68 },
    new Location { Id = 73, Name = "Palermo", IsCountry = false, ParentId = 68 },
    new Location { Id = 74, Name = "Genoa", IsCountry = false, ParentId = 68 },
    new Location { Id = 75, Name = "Bologna", IsCountry = false, ParentId = 68 },
    new Location { Id = 76, Name = "Florence", IsCountry = false, ParentId = 68 },
    new Location { Id = 77, Name = "Bari", IsCountry = false, ParentId = 68 },
    new Location { Id = 78, Name = "Catania", IsCountry = false, ParentId = 68 },
    new Location { Id = 79, Name = "Venice", IsCountry = false, ParentId = 68 },
    new Location { Id = 80, Name = "Verona", IsCountry = false, ParentId = 68 },
    new Location { Id = 81, Name = "Messina", IsCountry = false, ParentId = 68 },
    new Location { Id = 82, Name = "Padua", IsCountry = false, ParentId = 68 },
    
    // Spain
    new Location { Id = 83, Name = "Spain", IsCountry = true, ParentId = null },
    new Location { Id = 84, Name = "Madrid", IsCountry = false, ParentId = 83 },
    new Location { Id = 85, Name = "Barcelona", IsCountry = false, ParentId = 83 },
    new Location { Id = 86, Name = "Valencia", IsCountry = false, ParentId = 83 },
    new Location { Id = 87, Name = "Seville", IsCountry = false, ParentId = 83 },
    new Location { Id = 88, Name = "Zaragoza", IsCountry = false, ParentId = 83 },
    new Location { Id = 89, Name = "Malaga", IsCountry = false, ParentId = 83 },
    new Location { Id = 90, Name = "Murcia", IsCountry = false, ParentId = 83 },
    new Location { Id = 91, Name = "Palma", IsCountry = false, ParentId = 83 },
    new Location { Id = 92, Name = "Las Palmas", IsCountry = false, ParentId = 83 },
    new Location { Id = 93, Name = "Bilbao", IsCountry = false, ParentId = 83 },
    new Location { Id = 94, Name = "Alicante", IsCountry = false, ParentId = 83 },
    new Location { Id = 95, Name = "Cordoba", IsCountry = false, ParentId = 83 },
    new Location { Id = 96, Name = "Valladolid", IsCountry = false, ParentId = 83 },
    new Location { Id = 97, Name = "Vigo", IsCountry = false, ParentId = 83 },
    
    // United Kingdom
    new Location { Id = 98, Name = "United Kingdom", IsCountry = true, ParentId = null },
    new Location { Id = 99, Name = "London", IsCountry = false, ParentId = 98 },
    new Location { Id = 100, Name = "Birmingham", IsCountry = false, ParentId = 98 },
    new Location { Id = 101, Name = "Manchester", IsCountry = false, ParentId = 98 },
    new Location { Id = 102, Name = "Leeds", IsCountry = false, ParentId = 98 },
    new Location { Id = 103, Name = "Glasgow", IsCountry = false, ParentId = 98 },
    new Location { Id = 104, Name = "Liverpool", IsCountry = false, ParentId = 98 },
    new Location { Id = 105, Name = "Newcastle", IsCountry = false, ParentId = 98 },
    new Location { Id = 106, Name = "Sheffield", IsCountry = false, ParentId = 98 },
    new Location { Id = 107, Name = "Bristol", IsCountry = false, ParentId = 98 },
    new Location { Id = 108, Name = "Edinburgh", IsCountry = false, ParentId = 98 },
    new Location { Id = 109, Name = "Leicester", IsCountry = false, ParentId = 98 },
    new Location { Id = 110, Name = "Nottingham", IsCountry = false, ParentId = 98 },
    new Location { Id = 111, Name = "Cardiff", IsCountry = false, ParentId = 98 },
    new Location { Id = 112, Name = "Belfast", IsCountry = false, ParentId = 98 },
    
    // Japan
    new Location { Id = 113, Name = "Japan", IsCountry = true, ParentId = null },
    new Location { Id = 114, Name = "Tokyo", IsCountry = false, ParentId = 113 },
    new Location { Id = 115, Name = "Yokohama", IsCountry = false, ParentId = 113 },
    new Location { Id = 116, Name = "Osaka", IsCountry = false, ParentId = 113 },
    new Location { Id = 117, Name = "Nagoya", IsCountry = false, ParentId = 113 },
    new Location { Id = 118, Name = "Sapporo", IsCountry = false, ParentId = 113 },
    new Location { Id = 119, Name = "Fukuoka", IsCountry = false, ParentId = 113 },
    new Location { Id = 120, Name = "Kobe", IsCountry = false, ParentId = 113 },
    new Location { Id = 121, Name = "Kyoto", IsCountry = false, ParentId = 113 },
    new Location { Id = 122, Name = "Kawasaki", IsCountry = false, ParentId = 113 },
    new Location { Id = 123, Name = "Saitama", IsCountry = false, ParentId = 113 },
    new Location { Id = 124, Name = "Hiroshima", IsCountry = false, ParentId = 113 },
    new Location { Id = 125, Name = "Sendai", IsCountry = false, ParentId = 113 },
    new Location { Id = 126, Name = "Chiba", IsCountry = false, ParentId = 113 },
    new Location { Id = 127, Name = "Kitakyushu", IsCountry = false, ParentId = 113 },
    
    // Canada
    new Location { Id = 128, Name = "Canada", IsCountry = true, ParentId = null },
    new Location { Id = 129, Name = "Toronto", IsCountry = false, ParentId = 128 },
    new Location { Id = 130, Name = "Montreal", IsCountry = false, ParentId = 128 },
    new Location { Id = 131, Name = "Vancouver", IsCountry = false, ParentId = 128 },
    new Location { Id = 132, Name = "Calgary", IsCountry = false, ParentId = 128 },
    new Location { Id = 133, Name = "Edmonton", IsCountry = false, ParentId = 128 },
    new Location { Id = 134, Name = "Ottawa", IsCountry = false, ParentId = 128 },
    new Location { Id = 135, Name = "Winnipeg", IsCountry = false, ParentId = 128 },
    new Location { Id = 136, Name = "Quebec City", IsCountry = false, ParentId = 128 },
    new Location { Id = 137, Name = "Hamilton", IsCountry = false, ParentId = 128 },
    new Location { Id = 138, Name = "Kitchener", IsCountry = false, ParentId = 128 },
    new Location { Id = 139, Name = "London", IsCountry = false, ParentId = 128 },
    new Location { Id = 140, Name = "Victoria", IsCountry = false, ParentId = 128 },
    new Location { Id = 141, Name = "Halifax", IsCountry = false, ParentId = 128 },
    
    // Australia
    new Location { Id = 142, Name = "Australia", IsCountry = true, ParentId = null },
    new Location { Id = 143, Name = "Sydney", IsCountry = false, ParentId = 142 },
    new Location { Id = 144, Name = "Melbourne", IsCountry = false, ParentId = 142 },
    new Location { Id = 145, Name = "Brisbane", IsCountry = false, ParentId = 142 },
    new Location { Id = 146, Name = "Perth", IsCountry = false, ParentId = 142 },
    new Location { Id = 147, Name = "Adelaide", IsCountry = false, ParentId = 142 },
    new Location { Id = 148, Name = "Gold Coast", IsCountry = false, ParentId = 142 },
    new Location { Id = 149, Name = "Canberra", IsCountry = false, ParentId = 142 },
    new Location { Id = 150, Name = "Newcastle", IsCountry = false, ParentId = 142 },
    new Location { Id = 151, Name = "Wollongong", IsCountry = false, ParentId = 142 },
    new Location { Id = 152, Name = "Hobart", IsCountry = false, ParentId = 142 },
    new Location { Id = 153, Name = "Darwin", IsCountry = false, ParentId = 142 },
    
    // China
    new Location { Id = 154, Name = "China", IsCountry = true, ParentId = null },
    new Location { Id = 155, Name = "Beijing", IsCountry = false, ParentId = 154 },
    new Location { Id = 156, Name = "Shanghai", IsCountry = false, ParentId = 154 },
    new Location { Id = 157, Name = "Guangzhou", IsCountry = false, ParentId = 154 },
    new Location { Id = 158, Name = "Shenzhen", IsCountry = false, ParentId = 154 },
    new Location { Id = 159, Name = "Chengdu", IsCountry = false, ParentId = 154 },
    new Location { Id = 160, Name = "Chongqing", IsCountry = false, ParentId = 154 },
    new Location { Id = 161, Name = "Tianjin", IsCountry = false, ParentId = 154 },
    new Location { Id = 162, Name = "Wuhan", IsCountry = false, ParentId = 154 },
    new Location { Id = 163, Name = "Hangzhou", IsCountry = false, ParentId = 154 },
    new Location { Id = 164, Name = "Xi'an", IsCountry = false, ParentId = 154 },
    new Location { Id = 165, Name = "Nanjing", IsCountry = false, ParentId = 154 },
    new Location { Id = 166, Name = "Suzhou", IsCountry = false, ParentId = 154 },
    
    // Russia
    new Location { Id = 167, Name = "Russia", IsCountry = true, ParentId = null },
    new Location { Id = 168, Name = "Moscow", IsCountry = false, ParentId = 167 },
    new Location { Id = 169, Name = "Saint Petersburg", IsCountry = false, ParentId = 167 },
    new Location { Id = 170, Name = "Novosibirsk", IsCountry = false, ParentId = 167 },
    new Location { Id = 171, Name = "Yekaterinburg", IsCountry = false, ParentId = 167 },
    new Location { Id = 172, Name = "Kazan", IsCountry = false, ParentId = 167 },
    new Location { Id = 173, Name = "Nizhny Novgorod", IsCountry = false, ParentId = 167 },
    new Location { Id = 174, Name = "Chelyabinsk", IsCountry = false, ParentId = 167 },
    new Location { Id = 175, Name = "Samara", IsCountry = false, ParentId = 167 },
    new Location { Id = 176, Name = "Omsk", IsCountry = false, ParentId = 167 },
    new Location { Id = 177, Name = "Rostov-on-Don", IsCountry = false, ParentId = 167 },
    new Location { Id = 178, Name = "Ufa", IsCountry = false, ParentId = 167 },
    new Location { Id = 179, Name = "Krasnoyarsk", IsCountry = false, ParentId = 167 },
    
    // Brazil
    new Location { Id = 180, Name = "Brazil", IsCountry = true, ParentId = null },
    new Location { Id = 181, Name = "Sao Paulo", IsCountry = false, ParentId = 180 },
    new Location { Id = 182, Name = "Rio de Janeiro", IsCountry = false, ParentId = 180 },
    new Location { Id = 183, Name = "Brasilia", IsCountry = false, ParentId = 180 },
    new Location { Id = 184, Name = "Salvador", IsCountry = false, ParentId = 180 },
    new Location { Id = 185, Name = "Fortaleza", IsCountry = false, ParentId = 180 },
    new Location { Id = 186, Name = "Belo Horizonte", IsCountry = false, ParentId = 180 },
    new Location { Id = 187, Name = "Manaus", IsCountry = false, ParentId = 180 },
    new Location { Id = 188, Name = "Curitiba", IsCountry = false, ParentId = 180 },
    new Location { Id = 189, Name = "Recife", IsCountry = false, ParentId = 180 },
    new Location { Id = 190, Name = "Porto Alegre", IsCountry = false, ParentId = 180 },
    
    // India
    new Location { Id = 191, Name = "India", IsCountry = true, ParentId = null },
    new Location { Id = 192, Name = "Mumbai", IsCountry = false, ParentId = 191 },
    new Location { Id = 193, Name = "Delhi", IsCountry = false, ParentId = 191 },
    new Location { Id = 194, Name = "Bangalore", IsCountry = false, ParentId = 191 },
    new Location { Id = 195, Name = "Hyderabad", IsCountry = false, ParentId = 191 },
    new Location { Id = 196, Name = "Chennai", IsCountry = false, ParentId = 191 },
    new Location { Id = 197, Name = "Kolkata", IsCountry = false, ParentId = 191 },
    new Location { Id = 198, Name = "Pune", IsCountry = false, ParentId = 191 },
    new Location { Id = 199, Name = "Ahmedabad", IsCountry = false, ParentId = 191 },
    new Location { Id = 200, Name = "Jaipur", IsCountry = false, ParentId = 191 },
    new Location { Id = 201, Name = "Surat", IsCountry = false, ParentId = 191 },
    new Location { Id = 202, Name = "Lucknow", IsCountry = false, ParentId = 191 },
    
    // Mexico
    new Location { Id = 203, Name = "Mexico", IsCountry = true, ParentId = null },
    new Location { Id = 204, Name = "Mexico City", IsCountry = false, ParentId = 203 },
    new Location { Id = 205, Name = "Guadalajara", IsCountry = false, ParentId = 203 },
    new Location { Id = 206, Name = "Monterrey", IsCountry = false, ParentId = 203 },
    new Location { Id = 207, Name = "Puebla", IsCountry = false, ParentId = 203 },
    new Location { Id = 208, Name = "Tijuana", IsCountry = false, ParentId = 203 },
    new Location { Id = 209, Name = "Leon", IsCountry = false, ParentId = 203 },
    new Location { Id = 210, Name = "Juarez", IsCountry = false, ParentId = 203 },
    new Location { Id = 211, Name = "Zapopan", IsCountry = false, ParentId = 203 },
    new Location { Id = 212, Name = "Merida", IsCountry = false, ParentId = 203 },
    new Location { Id = 213, Name = "Cancun", IsCountry = false, ParentId = 203 },
    
    // South Korea
    new Location { Id = 214, Name = "South Korea", IsCountry = true, ParentId = null },
    new Location { Id = 215, Name = "Seoul", IsCountry = false, ParentId = 214 },
    new Location { Id = 216, Name = "Busan", IsCountry = false, ParentId = 214 },
    new Location { Id = 217, Name = "Incheon", IsCountry = false, ParentId = 214 },
    new Location { Id = 218, Name = "Daegu", IsCountry = false, ParentId = 214 },
    new Location { Id = 219, Name = "Daejeon", IsCountry = false, ParentId = 214 },
    new Location { Id = 220, Name = "Gwangju", IsCountry = false, ParentId = 214 },
    new Location { Id = 221, Name = "Suwon", IsCountry = false, ParentId = 214 },
    new Location { Id = 222, Name = "Ulsan", IsCountry = false, ParentId = 214 },
    
    // Turkey
    new Location { Id = 223, Name = "Turkey", IsCountry = true, ParentId = null },
    new Location { Id = 224, Name = "Istanbul", IsCountry = false, ParentId = 223 },
    new Location { Id = 225, Name = "Ankara", IsCountry = false, ParentId = 223 },
    new Location { Id = 226, Name = "Izmir", IsCountry = false, ParentId = 223 },
    new Location { Id = 227, Name = "Bursa", IsCountry = false, ParentId = 223 },
    new Location { Id = 228, Name = "Antalya", IsCountry = false, ParentId = 223 },
    new Location { Id = 229, Name = "Adana", IsCountry = false, ParentId = 223 },
    new Location { Id = 230, Name = "Gaziantep", IsCountry = false, ParentId = 223 },
    new Location { Id = 231, Name = "Konya", IsCountry = false, ParentId = 223 },
    
    // Netherlands
    new Location { Id = 232, Name = "Netherlands", IsCountry = true, ParentId = null },
    new Location { Id = 233, Name = "Amsterdam", IsCountry = false, ParentId = 232 },
    new Location { Id = 234, Name = "Rotterdam", IsCountry = false, ParentId = 232 },
    new Location { Id = 235, Name = "The Hague", IsCountry = false, ParentId = 232 },
    new Location { Id = 236, Name = "Utrecht", IsCountry = false, ParentId = 232 },
    new Location { Id = 237, Name = "Eindhoven", IsCountry = false, ParentId = 232 },
    new Location { Id = 238, Name = "Tilburg", IsCountry = false, ParentId = 232 },
    new Location { Id = 239, Name = "Groningen", IsCountry = false, ParentId = 232 },
    
    // Poland
    new Location { Id = 240, Name = "Poland", IsCountry = true, ParentId = null },
    new Location { Id = 241, Name = "Warsaw", IsCountry = false, ParentId = 240 },
    new Location { Id = 242, Name = "Krakow", IsCountry = false, ParentId = 240 },
    new Location { Id = 243, Name = "Lodz", IsCountry = false, ParentId = 240 },
    new Location { Id = 244, Name = "Wroclaw", IsCountry = false, ParentId = 240 },
    new Location { Id = 245, Name = "Poznan", IsCountry = false, ParentId = 240 },
    new Location { Id = 246, Name = "Gdansk", IsCountry = false, ParentId = 240 },
    new Location { Id = 247, Name = "Szczecin", IsCountry = false, ParentId = 240 },
    
    // Argentina
    new Location { Id = 248, Name = "Argentina", IsCountry = true, ParentId = null },
    new Location { Id = 249, Name = "Buenos Aires", IsCountry = false, ParentId = 248 },
    new Location { Id = 250, Name = "Cordoba", IsCountry = false, ParentId = 248 },
    new Location { Id = 251, Name = "Rosario", IsCountry = false, ParentId = 248 },
    new Location { Id = 252, Name = "Mendoza", IsCountry = false, ParentId = 248 },
    new Location { Id = 253, Name = "La Plata", IsCountry = false, ParentId = 248 },
    new Location { Id = 254, Name = "San Miguel de Tucuman", IsCountry = false, ParentId = 248 },
    
    // Sweden
    new Location { Id = 255, Name = "Sweden", IsCountry = true, ParentId = null },
    new Location { Id = 256, Name = "Stockholm", IsCountry = false, ParentId = 255 },
    new Location { Id = 257, Name = "Gothenburg", IsCountry = false, ParentId = 255 },
    new Location { Id = 258, Name = "Malmo", IsCountry = false, ParentId = 255 },
    new Location { Id = 259, Name = "Uppsala", IsCountry = false, ParentId = 255 },
    new Location { Id = 260, Name = "Vasteras", IsCountry = false, ParentId = 255 },
    
    // Switzerland
    new Location { Id = 261, Name = "Switzerland", IsCountry = true, ParentId = null },
    new Location { Id = 262, Name = "Zurich", IsCountry = false, ParentId = 261 },
    new Location { Id = 263, Name = "Geneva", IsCountry = false, ParentId = 261 },
    new Location { Id = 264, Name = "Basel", IsCountry = false, ParentId = 261 },
    new Location { Id = 265, Name = "Lausanne", IsCountry = false, ParentId = 261 },
new Location { Id = 266, Name = "Bern", IsCountry = false, ParentId = 261 },
    new Location { Id = 267, Name = "Winterthur", IsCountry = false, ParentId = 261 },
    
    // Austria
    new Location { Id = 268, Name = "Austria", IsCountry = true, ParentId = null },
    new Location { Id = 269, Name = "Vienna", IsCountry = false, ParentId = 268 },
    new Location { Id = 270, Name = "Graz", IsCountry = false, ParentId = 268 },
    new Location { Id = 271, Name = "Linz", IsCountry = false, ParentId = 268 },
    new Location { Id = 272, Name = "Salzburg", IsCountry = false, ParentId = 268 },
    new Location { Id = 273, Name = "Innsbruck", IsCountry = false, ParentId = 268 },
    
    // Belgium
    new Location { Id = 274, Name = "Belgium", IsCountry = true, ParentId = null },
    new Location { Id = 275, Name = "Brussels", IsCountry = false, ParentId = 274 },
    new Location { Id = 276, Name = "Antwerp", IsCountry = false, ParentId = 274 },
    new Location { Id = 277, Name = "Ghent", IsCountry = false, ParentId = 274 },
    new Location { Id = 278, Name = "Charleroi", IsCountry = false, ParentId = 274 },
    new Location { Id = 279, Name = "Liege", IsCountry = false, ParentId = 274 },
    new Location { Id = 280, Name = "Bruges", IsCountry = false, ParentId = 274 },
    
    // Portugal
    new Location { Id = 281, Name = "Portugal", IsCountry = true, ParentId = null },
    new Location { Id = 282, Name = "Lisbon", IsCountry = false, ParentId = 281 },
    new Location { Id = 283, Name = "Porto", IsCountry = false, ParentId = 281 },
    new Location { Id = 284, Name = "Vila Nova de Gaia", IsCountry = false, ParentId = 281 },
    new Location { Id = 285, Name = "Amadora", IsCountry = false, ParentId = 281 },
    new Location { Id = 286, Name = "Braga", IsCountry = false, ParentId = 281 },
    new Location { Id = 287, Name = "Funchal", IsCountry = false, ParentId = 281 },
    
    // Greece
    new Location { Id = 288, Name = "Greece", IsCountry = true, ParentId = null },
    new Location { Id = 289, Name = "Athens", IsCountry = false, ParentId = 288 },
    new Location { Id = 290, Name = "Thessaloniki", IsCountry = false, ParentId = 288 },
    new Location { Id = 291, Name = "Patras", IsCountry = false, ParentId = 288 },
    new Location { Id = 292, Name = "Heraklion", IsCountry = false, ParentId = 288 },
    new Location { Id = 293, Name = "Larissa", IsCountry = false, ParentId = 288 },
    
    // Czech Republic
    new Location { Id = 294, Name = "Czech Republic", IsCountry = true, ParentId = null },
    new Location { Id = 295, Name = "Prague", IsCountry = false, ParentId = 294 },
    new Location { Id = 296, Name = "Brno", IsCountry = false, ParentId = 294 },
    new Location { Id = 297, Name = "Ostrava", IsCountry = false, ParentId = 294 },
    new Location { Id = 298, Name = "Plzen", IsCountry = false, ParentId = 294 },
    new Location { Id = 299, Name = "Liberec", IsCountry = false, ParentId = 294 },
    
    // Romania
    new Location { Id = 300, Name = "Romania", IsCountry = true, ParentId = null },
    new Location { Id = 301, Name = "Bucharest", IsCountry = false, ParentId = 300 },
    new Location { Id = 302, Name = "Cluj-Napoca", IsCountry = false, ParentId = 300 },
    new Location { Id = 303, Name = "Timisoara", IsCountry = false, ParentId = 300 },
    new Location { Id = 304, Name = "Iasi", IsCountry = false, ParentId = 300 },
    new Location { Id = 305, Name = "Constanta", IsCountry = false, ParentId = 300 },
    new Location { Id = 306, Name = "Craiova", IsCountry = false, ParentId = 300 },
    
    // Hungary
    new Location { Id = 307, Name = "Hungary", IsCountry = true, ParentId = null },
    new Location { Id = 308, Name = "Budapest", IsCountry = false, ParentId = 307 },
    new Location { Id = 309, Name = "Debrecen", IsCountry = false, ParentId = 307 },
    new Location { Id = 310, Name = "Szeged", IsCountry = false, ParentId = 307 },
    new Location { Id = 311, Name = "Miskolc", IsCountry = false, ParentId = 307 },
    new Location { Id = 312, Name = "Pecs", IsCountry = false, ParentId = 307 },
    
    // Denmark
    new Location { Id = 313, Name = "Denmark", IsCountry = true, ParentId = null },
    new Location { Id = 314, Name = "Copenhagen", IsCountry = false, ParentId = 313 },
    new Location { Id = 315, Name = "Aarhus", IsCountry = false, ParentId = 313 },
    new Location { Id = 316, Name = "Odense", IsCountry = false, ParentId = 313 },
    new Location { Id = 317, Name = "Aalborg", IsCountry = false, ParentId = 313 },
    new Location { Id = 318, Name = "Esbjerg", IsCountry = false, ParentId = 313 },
    
    // Finland
    new Location { Id = 319, Name = "Finland", IsCountry = true, ParentId = null },
    new Location { Id = 320, Name = "Helsinki", IsCountry = false, ParentId = 319 },
    new Location { Id = 321, Name = "Espoo", IsCountry = false, ParentId = 319 },
    new Location { Id = 322, Name = "Tampere", IsCountry = false, ParentId = 319 },
    new Location { Id = 323, Name = "Vantaa", IsCountry = false, ParentId = 319 },
    new Location { Id = 324, Name = "Oulu", IsCountry = false, ParentId = 319 },
    new Location { Id = 325, Name = "Turku", IsCountry = false, ParentId = 319 },
    
    // Norway
    new Location { Id = 326, Name = "Norway", IsCountry = true, ParentId = null },
    new Location { Id = 327, Name = "Oslo", IsCountry = false, ParentId = 326 },
    new Location { Id = 328, Name = "Bergen", IsCountry = false, ParentId = 326 },
    new Location { Id = 329, Name = "Stavanger", IsCountry = false, ParentId = 326 },
    new Location { Id = 330, Name = "Trondheim", IsCountry = false, ParentId = 326 },
    new Location { Id = 331, Name = "Drammen", IsCountry = false, ParentId = 326 },
    
    // Ireland
    new Location { Id = 332, Name = "Ireland", IsCountry = true, ParentId = null },
    new Location { Id = 333, Name = "Dublin", IsCountry = false, ParentId = 332 },
    new Location { Id = 334, Name = "Cork", IsCountry = false, ParentId = 332 },
    new Location { Id = 335, Name = "Limerick", IsCountry = false, ParentId = 332 },
    new Location { Id = 336, Name = "Galway", IsCountry = false, ParentId = 332 },
    new Location { Id = 337, Name = "Waterford", IsCountry = false, ParentId = 332 },
    
    // New Zealand
    new Location { Id = 338, Name = "New Zealand", IsCountry = true, ParentId = null },
    new Location { Id = 339, Name = "Auckland", IsCountry = false, ParentId = 338 },
    new Location { Id = 340, Name = "Wellington", IsCountry = false, ParentId = 338 },
    new Location { Id = 341, Name = "Christchurch", IsCountry = false, ParentId = 338 },
    new Location { Id = 342, Name = "Hamilton", IsCountry = false, ParentId = 338 },
    new Location { Id = 343, Name = "Tauranga", IsCountry = false, ParentId = 338 },
    new Location { Id = 344, Name = "Dunedin", IsCountry = false, ParentId = 338 },
    
    // South Africa
    new Location { Id = 345, Name = "South Africa", IsCountry = true, ParentId = null },
    new Location { Id = 346, Name = "Johannesburg", IsCountry = false, ParentId = 345 },
    new Location { Id = 347, Name = "Cape Town", IsCountry = false, ParentId = 345 },
    new Location { Id = 348, Name = "Durban", IsCountry = false, ParentId = 345 },
    new Location { Id = 349, Name = "Pretoria", IsCountry = false, ParentId = 345 },
    new Location { Id = 350, Name = "Port Elizabeth", IsCountry = false, ParentId = 345 },
    new Location { Id = 351, Name = "Bloemfontein", IsCountry = false, ParentId = 345 },
    
    // Egypt
    new Location { Id = 352, Name = "Egypt", IsCountry = true, ParentId = null },
    new Location { Id = 353, Name = "Cairo", IsCountry = false, ParentId = 352 },
    new Location { Id = 354, Name = "Alexandria", IsCountry = false, ParentId = 352 },
    new Location { Id = 355, Name = "Giza", IsCountry = false, ParentId = 352 },
    new Location { Id = 356, Name = "Shubra El Kheima", IsCountry = false, ParentId = 352 },
    new Location { Id = 357, Name = "Port Said", IsCountry = false, ParentId = 352 },
    new Location { Id = 358, Name = "Suez", IsCountry = false, ParentId = 352 },
    
    // Saudi Arabia
    new Location { Id = 359, Name = "Saudi Arabia", IsCountry = true, ParentId = null },
    new Location { Id = 360, Name = "Riyadh", IsCountry = false, ParentId = 359 },
    new Location { Id = 361, Name = "Jeddah", IsCountry = false, ParentId = 359 },
    new Location { Id = 362, Name = "Mecca", IsCountry = false, ParentId = 359 },
    new Location { Id = 363, Name = "Medina", IsCountry = false, ParentId = 359 },
    new Location { Id = 364, Name = "Dammam", IsCountry = false, ParentId = 359 },
    new Location { Id = 365, Name = "Khobar", IsCountry = false, ParentId = 359 },
    
    // UAE
    new Location { Id = 366, Name = "United Arab Emirates", IsCountry = true, ParentId = null },
    new Location { Id = 367, Name = "Dubai", IsCountry = false, ParentId = 366 },
    new Location { Id = 368, Name = "Abu Dhabi", IsCountry = false, ParentId = 366 },
    new Location { Id = 369, Name = "Sharjah", IsCountry = false, ParentId = 366 },
    new Location { Id = 370, Name = "Ajman", IsCountry = false, ParentId = 366 },
    new Location { Id = 371, Name = "Ras Al Khaimah", IsCountry = false, ParentId = 366 },
    new Location { Id = 372, Name = "Fujairah", IsCountry = false, ParentId = 366 },
    
    // Singapore
    new Location { Id = 373, Name = "Singapore", IsCountry = true, ParentId = null },
    new Location { Id = 374, Name = "Singapore City", IsCountry = false, ParentId = 373 },
    
    // Malaysia
    new Location { Id = 375, Name = "Malaysia", IsCountry = true, ParentId = null },
    new Location { Id = 376, Name = "Kuala Lumpur", IsCountry = false, ParentId = 375 },
    new Location { Id = 377, Name = "George Town", IsCountry = false, ParentId = 375 },
    new Location { Id = 378, Name = "Johor Bahru", IsCountry = false, ParentId = 375 },
    new Location { Id = 379, Name = "Ipoh", IsCountry = false, ParentId = 375 },
    new Location { Id = 380, Name = "Shah Alam", IsCountry = false, ParentId = 375 },
    
    // Thailand
    new Location { Id = 381, Name = "Thailand", IsCountry = true, ParentId = null },
    new Location { Id = 382, Name = "Bangkok", IsCountry = false, ParentId = 381 },
    new Location { Id = 383, Name = "Chiang Mai", IsCountry = false, ParentId = 381 },
    new Location { Id = 384, Name = "Phuket", IsCountry = false, ParentId = 381 },
    new Location { Id = 385, Name = "Pattaya", IsCountry = false, ParentId = 381 },
    new Location { Id = 386, Name = "Nonthaburi", IsCountry = false, ParentId = 381 },
    new Location { Id = 387, Name = "Udon Thani", IsCountry = false, ParentId = 381 },
    
    // Vietnam
    new Location { Id = 388, Name = "Vietnam", IsCountry = true, ParentId = null },
    new Location { Id = 389, Name = "Ho Chi Minh City", IsCountry = false, ParentId = 388 },
    new Location { Id = 390, Name = "Hanoi", IsCountry = false, ParentId = 388 },
    new Location { Id = 391, Name = "Da Nang", IsCountry = false, ParentId = 388 },
    new Location { Id = 392, Name = "Haiphong", IsCountry = false, ParentId = 388 },
    new Location { Id = 393, Name = "Can Tho", IsCountry = false, ParentId = 388 },
    new Location { Id = 394, Name = "Bien Hoa", IsCountry = false, ParentId = 388 },
    
    // Philippines
    new Location { Id = 395, Name = "Philippines", IsCountry = true, ParentId = null },
    new Location { Id = 396, Name = "Manila", IsCountry = false, ParentId = 395 },
    new Location { Id = 397, Name = "Quezon City", IsCountry = false, ParentId = 395 },
    new Location { Id = 398, Name = "Davao City", IsCountry = false, ParentId = 395 },
    new Location { Id = 399, Name = "Cebu City", IsCountry = false, ParentId = 395 },
    new Location { Id = 400, Name = "Makati", IsCountry = false, ParentId = 395 },
    new Location { Id = 401, Name = "Taguig", IsCountry = false, ParentId = 395 },
    
    // Indonesia
    new Location { Id = 402, Name = "Indonesia", IsCountry = true, ParentId = null },
    new Location { Id = 403, Name = "Jakarta", IsCountry = false, ParentId = 402 },
    new Location { Id = 404, Name = "Surabaya", IsCountry = false, ParentId = 402 },
    new Location { Id = 405, Name = "Bandung", IsCountry = false, ParentId = 402 },
    new Location { Id = 406, Name = "Bekasi", IsCountry = false, ParentId = 402 },
    new Location { Id = 407, Name = "Medan", IsCountry = false, ParentId = 402 },
    new Location { Id = 408, Name = "Tangerang", IsCountry = false, ParentId = 402 },
    new Location { Id = 409, Name = "Depok", IsCountry = false, ParentId = 402 },
    new Location { Id = 410, Name = "Semarang", IsCountry = false, ParentId = 402 },
    new Location { Id = 411, Name = "Palembang", IsCountry = false, ParentId = 402 },
    new Location { Id = 412, Name = "Bali", IsCountry = false, ParentId = 402 },
    
    // Chile
    new Location { Id = 413, Name = "Chile", IsCountry = true, ParentId = null },
    new Location { Id = 414, Name = "Santiago", IsCountry = false, ParentId = 413 },
    new Location { Id = 415, Name = "Valparaiso", IsCountry = false, ParentId = 413 },
    new Location { Id = 416, Name = "Concepcion", IsCountry = false, ParentId = 413 },
    new Location { Id = 417, Name = "La Serena", IsCountry = false, ParentId = 413 },
    new Location { Id = 418, Name = "Antofagasta", IsCountry = false, ParentId = 413 },
    
    // Colombia
    new Location { Id = 419, Name = "Colombia", IsCountry = true, ParentId = null },
    new Location { Id = 420, Name = "Bogota", IsCountry = false, ParentId = 419 },
    new Location { Id = 421, Name = "Medellin", IsCountry = false, ParentId = 419 },
    new Location { Id = 422, Name = "Cali", IsCountry = false, ParentId = 419 },
    new Location { Id = 423, Name = "Barranquilla", IsCountry = false, ParentId = 419 },
    new Location { Id = 424, Name = "Cartagena", IsCountry = false, ParentId = 419 },
    new Location { Id = 425, Name = "Cucuta", IsCountry = false, ParentId = 419 },
    
    // Peru
    new Location { Id = 426, Name = "Peru", IsCountry = true, ParentId = null },
    new Location { Id = 427, Name = "Lima", IsCountry = false, ParentId = 426 },
    new Location { Id = 428, Name = "Arequipa", IsCountry = false, ParentId = 426 },
    new Location { Id = 429, Name = "Trujillo", IsCountry = false, ParentId = 426 },
    new Location { Id = 430, Name = "Chiclayo", IsCountry = false, ParentId = 426 },
    new Location { Id = 431, Name = "Cusco", IsCountry = false, ParentId = 426 },
    
    // Ukraine
    new Location { Id = 432, Name = "Ukraine", IsCountry = true, ParentId = null },
    new Location { Id = 433, Name = "Kyiv", IsCountry = false, ParentId = 432 },
    new Location { Id = 434, Name = "Kharkiv", IsCountry = false, ParentId = 432 },
    new Location { Id = 435, Name = "Odesa", IsCountry = false, ParentId = 432 },
    new Location { Id = 436, Name = "Dnipro", IsCountry = false, ParentId = 432 },
    new Location { Id = 437, Name = "Lviv", IsCountry = false, ParentId = 432 },
    new Location { Id = 438, Name = "Zaporizhzhia", IsCountry = false, ParentId = 432 },
    
    // Israel
    new Location { Id = 439, Name = "Israel", IsCountry = true, ParentId = null },
    new Location { Id = 440, Name = "Jerusalem", IsCountry = false, ParentId = 439 },
    new Location { Id = 441, Name = "Tel Aviv", IsCountry = false, ParentId = 439 },
    new Location { Id = 442, Name = "Haifa", IsCountry = false, ParentId = 439 },
    new Location { Id = 443, Name = "Rishon LeZion", IsCountry = false, ParentId = 439 },
    new Location { Id = 444, Name = "Petah Tikva", IsCountry = false, ParentId = 439 },
    
    // Morocco
    new Location { Id = 445, Name = "Morocco", IsCountry = true, ParentId = null },
    new Location { Id = 446, Name = "Casablanca", IsCountry = false, ParentId = 445 },
    new Location { Id = 447, Name = "Rabat", IsCountry = false, ParentId = 445 },
    new Location { Id = 448, Name = "Fes", IsCountry = false, ParentId = 445 },
    new Location { Id = 449, Name = "Marrakech", IsCountry = false, ParentId = 445 },
    new Location { Id = 450, Name = "Tangier", IsCountry = false, ParentId = 445 },
    
    // Kenya
    new Location { Id = 451, Name = "Kenya", IsCountry = true, ParentId = null },
    new Location { Id = 452, Name = "Nairobi", IsCountry = false, ParentId = 451 },
    new Location { Id = 453, Name = "Mombasa", IsCountry = false, ParentId = 451 },
    new Location { Id = 454, Name = "Kisumu", IsCountry = false, ParentId = 451 },
    new Location { Id = 455, Name = "Nakuru", IsCountry = false, ParentId = 451 },
    
    // Nigeria
    new Location { Id = 456, Name = "Nigeria", IsCountry = true, ParentId = null },
    new Location { Id = 457, Name = "Lagos", IsCountry = false, ParentId = 456 },
    new Location { Id = 458, Name = "Kano", IsCountry = false, ParentId = 456 },
    new Location { Id = 459, Name = "Ibadan", IsCountry = false, ParentId = 456 },
    new Location { Id = 460, Name = "Abuja", IsCountry = false, ParentId = 456 },
    new Location { Id = 461, Name = "Port Harcourt", IsCountry = false, ParentId = 456 },
    
    // Pakistan
    new Location { Id = 462, Name = "Pakistan", IsCountry = true, ParentId = null },
    new Location { Id = 463, Name = "Karachi", IsCountry = false, ParentId = 462 },
    new Location { Id = 464, Name = "Lahore", IsCountry = false, ParentId = 462 },
    new Location { Id = 465, Name = "Islamabad", IsCountry = false, ParentId = 462 },
    new Location { Id = 466, Name = "Rawalpindi", IsCountry = false, ParentId = 462 },
    new Location { Id = 467, Name = "Faisalabad", IsCountry = false, ParentId = 462 },
    
    // Bangladesh
    new Location { Id = 468, Name = "Bangladesh", IsCountry = true, ParentId = null },
    new Location { Id = 469, Name = "Dhaka", IsCountry = false, ParentId = 468 },
    new Location { Id = 470, Name = "Chittagong", IsCountry = false, ParentId = 468 },
    new Location { Id = 471, Name = "Khulna", IsCountry = false, ParentId = 468 },
    new Location { Id = 472, Name = "Rajshahi", IsCountry = false, ParentId = 468 },
    new Location { Id = 473, Name = "Sylhet", IsCountry = false, ParentId = 468 }
};

        modelBuilder.Entity<Location>().HasData(locations);

    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Publisher> Publishers { get; set; }

}
