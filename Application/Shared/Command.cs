using Domain.Entities.AuthorEntity.IRepository;
using Domain.Entities.BookAuthorEntity.IRepository;
using Domain.Entities.ProductEntity.IRepository;
using Domain.Entities.RoleEntity.IRepository;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
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
    protected IProductRepository _productRepository;
    protected IAuthorRepository _authorRepository;
    protected IBookAuthorRepository _bookAuthorRepository;

    public abstract Task<CommandExecutionResultGeneric<T>> ExecuteAsync();

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
        _productRepository = serviceProvider.GetService<IProductRepository>();
        _authorRepository = serviceProvider.GetService<IAuthorRepository>();
        _bookAuthorRepository = serviceProvider.GetService<IBookAuthorRepository>();
        _userManager = serviceProvider.GetService<UserManager<User>>();  // Add this line
    }
}

// Keep the original non-generic Command for backward compatibility
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