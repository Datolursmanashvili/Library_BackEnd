//using Domain.Entities.AuthorEntity.IRepository;
//using Domain.Entities.BookAuthorEntity.IRepository;
//using Domain.Entities.FileEntity.IRepository;
//using Domain.Entities.LocationEntity.IRepository;
//using Domain.Entities.ProductEntity.IRepository;
//using Domain.Entities.PublisherEntity.IRepository;
//using Domain.Entities.UserEntity.IRepository;
//using Infrastructure.Repositories;
//using Microsoft.Extensions.DependencyInjection;

//namespace Application.Shared;

//public class DI
//{
//    public static void DependecyResolver(IServiceCollection services)
//    {
//        services.AddScoped<IQueryExecutor, QueryExecutor>();
//        services.AddScoped<ICommandExecutor, CommandExecutor>();
//        services.AddScoped<IUserRepository, UserRepository>();
//        services.AddScoped<IAuthorRepository, AuthorRepository>();
//        services.AddScoped<IProductRepository, ProductRepository>();
//        services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
//        services.AddScoped<ILocationRepository, LocationRepository>();
//        services.AddScoped<IPublisherRepository, PublisherRepository>();
//        services.AddScoped<IFileClassRepository, FileClassRepository>();

//    }
//}


using Domain.Entities.AuthorEntity.IRepository;
using Domain.Entities.BookAuthorEntity.IRepository;
using Domain.Entities.FileEntity.IRepository;
using Domain.Entities.LocationEntity.IRepository;
using Domain.Entities.ProductEntity.IRepository;
using Domain.Entities.PublisherEntity.IRepository;
using Domain.Entities.UserEntity.IRepository;
using Domain.Shared.RedisModel.IRepository;
using Infrastructure.Caching;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Application.Shared;

public static class DI
{
    public static void DependecyResolver(
        IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterExecutors(services);
        RegisterRepositories(services);
        RegisterRedis(services, configuration);
    }

    private static void RegisterExecutors(IServiceCollection services)
    {
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IFileClassRepository, FileClassRepository>();
    }

    private static void RegisterRedis(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var redisSection = configuration.GetSection("Redis");
        var redisOptions = new RedisOptions
        {
            Host = redisSection["Host"]!,
            Port = int.Parse(redisSection["Port"]!),
            Password = redisSection["Password"]!,
            DefaultDatabase = int.Parse(redisSection["DefaultDatabase"] ?? "0"),
            SyncTimeout = int.Parse(redisSection["SyncTimeout"] ?? "5000")
        };

        services.AddSingleton(redisOptions);

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { { redisOptions.Host, redisOptions.Port } },
                Password = redisOptions.Password,
                DefaultDatabase = redisOptions.DefaultDatabase,
                SyncTimeout = redisOptions.SyncTimeout,
                AbortOnConnectFail = false
            };

            return ConnectionMultiplexer.Connect(options);
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
    }
}
