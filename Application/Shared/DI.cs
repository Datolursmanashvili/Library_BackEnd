using Domain.Entities.RoleEntity;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared;

public class DI
{
    public static void DependecyResolver(IServiceCollection services)
    {
        //services.AddIdentity<User, ApplicationRole>()
        //      .AddEntityFrameworkStores<ApplicationDbContext>()
        //      .AddDefaultTokenProviders();
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IUserRepository, UserRepository>();

    }

}