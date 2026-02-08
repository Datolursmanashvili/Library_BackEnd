using Domain.Entities.FileEntity.IRepository;
using Domain.Entities.RoleEntity.IRepository;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Domain.Shared.RedisModel.IRepository;
using FluentValidation;
using FluentValidation.Attributes;
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


    public async Task<CommandExecutionResultGeneric<T>> ExecuteAsync()
    {
        var validatorAttribute = (ValidatorAttribute)Attribute.GetCustomAttribute(
            this.GetType(), typeof(ValidatorAttribute));

        if (validatorAttribute?.ValidatorType != null)
        {
            var validatorInstance = Activator.CreateInstance(validatorAttribute.ValidatorType);

            if (validatorInstance is IValidator validator)
            {
                var validationResult = await validator.ValidateAsync(
                    new ValidationContext<object>(this)
                );

                if (!validationResult.IsValid)
                {
                    return await Fail<T>(
                        string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
                    );
                }
            }
        }

        return await ExecuteCommandLogicAsync();
    }

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

        userRepository = serviceProvider.GetService<IUserRepository>();
        _fileClassRepository = serviceProvider.GetService<IFileClassRepository>();
        _cacheService = serviceProvider.GetService<ICacheService>();

        _userManager = serviceProvider.GetService<UserManager<User>>(); // Add this line
    }

}

public abstract class Command : ResponseHelper
{
    protected ApplicationDbContext applicationDbContext;
    protected IServiceProvider ServiceProvider;
    protected IConfiguration Configuration;
    protected IRoleRepository RoleRepository;


    public abstract Task<CommandExecutionResult> ExecuteAsync();

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
        userRepository = serviceProvider.GetService<IUserRepository>();

        _userManager = serviceProvider.GetService<UserManager<User>>();  // Add this line

    }
}