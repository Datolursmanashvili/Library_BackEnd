using Domain.Entities.FileEntity.IRepository;
using Domain.Entities.RoleEntity.IRepository;
using Domain.Entities.UserEntity.IRepository;
using Domain.Shared.RedisModel;
using Domain.Shared.RedisModel.IRepository;
using FluentValidation;
using Infrastructure.Caching;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Reflection;

namespace Application.Shared;

public static class DI
{
    public static void DependecyResolver(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        RegisterExecutors(services);
        RegisterRepositories(services);
        //RegisterRedis(services, configuration);
    }

    private static void RegisterExecutors(IServiceCollection services)
    {
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileClassRepository, FileClassRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
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
