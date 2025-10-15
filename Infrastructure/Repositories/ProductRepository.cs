using Domain.Entities.ProductEntity;
using Domain.Entities.ProductEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : base(applicationDbContext, serviceProvider)
    {
    }

    // CREATE
    public async Task<CommandExecutionResult> CreateAsync(Product model)
    {
        try
        {
            var resultId = await Insert<Product, int>(model);
            return new CommandExecutionResult { Success = true, ErrorMessage = "პროდუქტი შექმნილია წარმატებით.", ResultId = resultId.ToString() };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // READ ALL
    public async Task<List<Product>?> GetAllAsync()
    {
        return await _ApplicationDbContext.Products.ToListAsync();
    }

    // READ BY ID
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _ApplicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    // UPDATE
    public async Task<CommandExecutionResult> UpdateAsync(Product model)
    {
        try
        {
            var existingProduct = await _ApplicationDbContext.Products.FindAsync(model.Id);
            if (existingProduct == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "პროდუქტი ვერ მოიძებნა." };

            existingProduct.Name = model.Name;
            existingProduct.Annotation = model.Annotation;
            existingProduct.ProductType = model.ProductType;
            existingProduct.ISBN = model.ISBN;
            existingProduct.ReleaseDate = model.ReleaseDate;
            existingProduct.PublisherId = model.PublisherId;

            _ApplicationDbContext.Products.Update(existingProduct);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "პროდუქტი განახლებულია წარმატებით." };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // DELETE
    public async Task<CommandExecutionResult> DeleteAsync(int id)
    {
        try
        {
            var product = await _ApplicationDbContext.Products.FindAsync(id);
            if (product == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "პროდუქტი ვერ მოიძებნა." };

            _ApplicationDbContext.Products.Remove(product);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "პროდუქტი წაშლილია წარმატებით." };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }
}
