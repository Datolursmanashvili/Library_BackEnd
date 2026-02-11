using Shared;

namespace Domain.Entities.UserEntity.IRepository;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task<List<User>> GetAllAsync();
    Task<RepositoryExecutionResult> Registration(User user, string RoleName);
    Task<RepositoryExecutionResult> UpdateAsyncUser(User user, bool userIsVacantion = false);
    Task<RepositoryExecutionResult> DeleteAsync(string id);
    Task<RepositoryExecutionResult> UpdateUsersRangeAsync(IEnumerable<User> user);
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
    Task<RepositoryExecutionResult> UpdateUser(User Edituser);
}
