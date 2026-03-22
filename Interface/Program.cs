using Application.Shared;
using Domain.Entities.RoleEntity;
using Domain.Entities.UserEntity;
using Infrastructure.DB;
using Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    static string? SqlTrust(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return connectionString;
        var csb = new SqlConnectionStringBuilder(connectionString)
        {
            TrustServerCertificate = true
        };
        return csb.ConnectionString;
    }

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var serilogConnection = SqlTrust(context.Configuration.GetConnectionString("DefaultConnection"))
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required for Serilog.");

        configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                connectionString: serilogConnection,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    SchemaName = "dbo",
                    AutoCreateSqlTable = true,
                    BatchPostingLimit = 1,
                    BatchPeriod = TimeSpan.FromSeconds(1)
                });
    });

    // Add services to the container.
    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();
    builder.Configuration.AddJsonFile("appsettings.json");
    var connectionString = SqlTrust(builder.Configuration.GetConnectionString("DefaultConnection"));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddIdentity<User, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    var jwtSecret = builder.Configuration["JwtSettings:SecretKey"];
    if (string.IsNullOrWhiteSpace(jwtSecret))
        throw new InvalidOperationException("JwtSettings:SecretKey must be set in configuration.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "Issuer",
            ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "Audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.Zero
        };
    });

    DI.DependecyResolver(builder.Services, builder.Configuration);

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    // --- Swagger с Bearer ---
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Library API",
            Version = "v1",
            Description =
                "Backend API for the library application. Authenticated endpoints require a JWT in the Authorization header (Bearer scheme). " +
                "Use User/Login to obtain a token. Responses use a common envelope (success flag, HTTP status code, errors, and payload in data) returned with the matching HTTP status.",
            Contact = new OpenApiContact
            {
                Name = "Library API",
            },
        });

        try
        {
            foreach (var xmlPath in Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly))
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        }
        catch
        {
            // XML docs are optional if referenced projects were not built yet
        }

        c.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);

        // Security Definition
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your token"
        });

        // Security Requirement
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

    var app = builder.Build();

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
        logger.LogInformation("Application started — Serilog MSSQL sink probe");
    });

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while creating or migrating the database.");
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
            c.RoutePrefix = string.Empty;
        });
    }


    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseCrudRequestLogging();

    app.MapControllers().RequireAuthorization();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
