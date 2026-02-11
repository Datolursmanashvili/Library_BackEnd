using Shared;

namespace Domain.Shared.BaseModel.IBaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>?> GetAllAsync();
        Task<RepositoryExecutionResult> CreateAsync(TEntity model);
        Task<RepositoryExecutionResult> UpdateAsync(TEntity model);
        Task<RepositoryExecutionResult> DeleteAsync(int id);
    }
}
