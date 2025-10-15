using Shared;

namespace Domain.Entities.UserEntity.IRepository;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task<List<User>> GetAllAsync();
    Task<CommandExecutionResult> Registration(User user, string RoleName);
    Task<CommandExecutionResult> UpdateAsyncUser(User user, bool userIsVacantion = false);
    Task<CommandExecutionResult> DeleteAsync(string id);
    Task<CommandExecutionResult> UpdateUsersRangeAsync(IEnumerable<User> user);
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
    Task<CommandExecutionResult> UpdateUser(User Edituser);
}
