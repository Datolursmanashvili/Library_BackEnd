using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Text;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    UserManager<User> _userManager;
    private readonly IConfiguration _config;


    public UserRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider, UserManager<User> userManager, IConfiguration config)
           : base(applicationDbContext, serviceProvider)
    {
        _userManager = userManager;
        _config = config;
    }


    public async Task<CommandExecutionResult> UpdateUser(User Edituser)
    {
        var user = await _userManager.FindByIdAsync(Edituser.Id);

        if (user == null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "User not found" };
        }

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return new CommandExecutionResult() { Success = true };
        }
        else
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = string.Join(", ", result.Errors) };
        }
    }


    public string Encrypt(string plainText)
    {

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public string Decrypt(string encryptedText)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(encryptedBytes);
        }
        catch (Exception)
        {

            return encryptedText;
        }

    }


    public async Task<CommandExecutionResult> Registration(User user, string RoleName)
    {
        var hashPrivateNumber = Encrypt(user.PNumber);

        if (user == null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Invalid user object" };
        }

        if (_ApplicationDbContext.Users.Any(x => x.PNumber == user.PNumber || x.PNumber == hashPrivateNumber))
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "ასეთი პირადი ნომერი უკვე არსებობს " };

        }

        if (await _userManager.FindByEmailAsync(user.Email) != null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = $"მომხმარებლის მეილი {user.Email} უკვე დაკავებულია" };
        }

        if (_ApplicationDbContext.Users.FirstOrDefault(x => x.IsActive && (x.Email == user.Email || x.UserName == user.UserName)).IsNotNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "მომხმარებლის სახელი უკვე დაკავებულია" };
        }



        var NewRole = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Name == RoleName);
        if (NewRole.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "Invalid Role Name" };
        }

        user.PNumber = hashPrivateNumber;
        try
        {
            if (_userManager == null)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = "User Manager is not initialized" };
            }
            user.Id = Guid.NewGuid().ToString();
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, user.PasswordHash);
            user.PasswordHash = hashedNewPassword;

            user.IsActive = true;
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded == false)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = result.Errors.ToString() };
            }



            var roleResult = await _userManager.AddToRoleAsync(_ApplicationDbContext.Users.First(x => x.Email == user.Email), NewRole.Name);
            if (roleResult.Succeeded == false)
            {
                return new CommandExecutionResult() { Success = false, ErrorMessage = roleResult.Errors.ToString() };
            }

            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }



    public async Task<CommandExecutionResult> UpdateAsyncUser(User user, bool userIsVacantion = false)
    {
        var employe = await _userManager.FindByIdAsync(user.Id.ToString());

        if (employe.IsNull())
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "User not found" };
        }

        try
        {
            employe.PNumber = user.PNumber;
            employe.FirstName = user.FirstName;
            employe.LastName = user.LastName;
            employe.PhoneNumber = user.PhoneNumber;
            employe.DepartmentId = user.DepartmentId;
            employe.IsActive = user.IsActive;

            await _userManager.UpdateAsync(employe);

            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult()
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<CommandExecutionResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = "User not found" };
        }

        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return new CommandExecutionResult() { Success = true };
        }
        else
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = string.Join(", ", result.Errors) };
        }
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public Task<List<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }



    public async Task<CommandExecutionResult> UpdateUsersRangeAsync(IEnumerable<User> user)
    {
        try
        {
            _ApplicationDbContext.Set<User>().UpdateRange(user);
            await _ApplicationDbContext.SaveChangesAsync();
            return new CommandExecutionResult { Success = true };

        }
        catch (Exception ex)
        {

            return new CommandExecutionResult { Success = false, ErrorMessage = String.Format("Error: {0}", ex.Message) };
        }

    }

}

public class ActiveUsersResultItem
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}