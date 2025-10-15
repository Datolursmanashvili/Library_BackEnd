using Domain.Shared.BaseModel;
using Infrastructure.DB;

namespace Infrastructure.Repositories;

public class BaseRepository
{
    protected ApplicationDbContext _ApplicationDbContext;
    protected IServiceProvider _ServiceProvider;
    public BaseRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
    {
        _ApplicationDbContext = applicationDbContext;
        _ServiceProvider = serviceProvider;
    }

    protected async Task Delete<TEntity>(TEntity entity) where TEntity : class
    {
        _ApplicationDbContext.Set<TEntity>().Remove(entity);

        await Save();
    }
    protected async Task DeleteRange<TEntity>(List<TEntity> entities) where TEntity : class
    {
        _ApplicationDbContext.Set<TEntity>().RemoveRange(entities);

        await Save();
    }

    protected async Task<TId> Insert<TEntity, TId>(TEntity entity) where TEntity : BaseEntity<TId>
    {
        try
        {
            await _ApplicationDbContext.Set<TEntity>().AddAsync(entity);
            await Save();

            return entity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Something went wrong: " + ex.Message);
        }

    }

    protected async Task Insert<TEntity>(TEntity entity) where TEntity : class
    {
        try
        {
            await _ApplicationDbContext.Set<TEntity>().AddAsync(entity);
            await Save();

        }
        catch (Exception ex)
        {
            throw new Exception("Something went wrong: " + ex.Message);
        }

    }

    protected async Task InsertRange<TEntity>(List<TEntity> entities) where TEntity : class
    {
        try
        {
            await _ApplicationDbContext.Set<TEntity>().AddRangeAsync(entities);

            await Save();
        }
        catch (Exception ex)
        {

            throw new Exception("Something went wrong: " + ex.Message);
        }

    }


    protected async Task Update<TEntity>(TEntity entity) where TEntity : class
    {
        _ApplicationDbContext.Set<TEntity>().Update(entity);

        await Save();
    }

    protected async Task UpdateRange<TEntity>(List<TEntity> entities) where TEntity : class
    {
        _ApplicationDbContext.Set<TEntity>().UpdateRange(entities);

        await Save();
    }


    public async Task<TEntity> OfId<TEntity>(int id) where TEntity : class
    {
        return await _ApplicationDbContext.Set<TEntity>().FindAsync(id);
    }
    private async Task Save()
    {
        await _ApplicationDbContext.SaveChangesAsync();
    }
}
