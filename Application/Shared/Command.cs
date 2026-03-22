using Domain.Entities.FileEntity.IRepository;
using Domain.Entities.RoleEntity.IRepository;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Domain.Shared.RedisModel.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Application.Shared;

public abstract class Command<T> : ResponseHelper
{
    protected ApplicationDbContext applicationDbContext;
    protected IServiceProvider ServiceProvider;
    protected IConfiguration Configuration;
    protected IRoleRepository RoleRepository;
    protected IFileClassRepository _fileClassRepository;
    protected ICacheService _cacheService;

    public abstract Task<CommandExecutionResultGeneric<T>> ExecuteCommandLogicAsync();

    protected IUserRepository userRepository;
    protected UserManager<User> _userManager;

    protected string? UserId;
    protected string? Username;


    public Task<CommandExecutionResultGeneric<T>> ExecuteAsync() =>
        ExecuteCommandLogicAsync();

    public void Resolve(ApplicationDbContext applicationContext, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        applicationDbContext = applicationContext;

        var user = ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;

        if (user.Claims.Any())
        {
            Username = user.Claims.First(i => i.Type == "UserName").Value;
            UserId = user.Claims.First(i => i.Type == "UserId").Value;
        }

        userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        _fileClassRepository = serviceProvider.GetRequiredService<IFileClassRepository>();
        _cacheService = serviceProvider.GetService<ICacheService>();
        RoleRepository = serviceProvider.GetRequiredService<IRoleRepository>();

        _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    }

}

public abstract class Command : ResponseHelper
{
    protected ApplicationDbContext applicationDbContext;
    protected IServiceProvider ServiceProvider;
    protected IConfiguration Configuration;
    protected IRoleRepository RoleRepository;


    public abstract Task<RepositoryExecutionResult> ExecuteAsync();

    protected IUserRepository userRepository;
    protected UserManager<User> _userManager;

    protected string? UserId;
    protected string? Username;
    public void Resolve(ApplicationDbContext applicationContext, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        applicationDbContext = applicationContext;

        var user = ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;

        if (user.Claims.Any())
        {
            Username = user.Claims.First(i => i.Type == "UserName").Value;
            UserId = user.Claims.First(i => i.Type == "UserId").Value;
        }
        userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        RoleRepository = serviceProvider.GetRequiredService<IRoleRepository>();

        _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    }
}