using Shared;

namespace Domain.Shared.BaseModel.IBaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>?> GetAllAsync();
        Task<CommandExecutionResult> CreateAsync(TEntity model);
        Task<CommandExecutionResult> UpdateAsync(TEntity model);
        Task<CommandExecutionResult> DeleteAsync(int id);
    }
}
