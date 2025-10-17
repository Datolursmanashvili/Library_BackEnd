using Domain.Entities.AuthorEntity.IRepository;
using Domain.Entities.BookAuthorEntity.IRepository;
using Domain.Entities.LocationEntity.IRepository;
using Domain.Entities.ProductEntity.IRepository;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared;

public class DI
{
    public static void DependecyResolver(IServiceCollection services)
    {
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
    }
}