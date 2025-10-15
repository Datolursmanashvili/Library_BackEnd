using Application.Shared;
using Shared;

namespace Application.Queries.ProductQueries
{
    public class GetProductByIdQuery : Query<ProductQueryResultItem>
    {
        public int Id { get; set; }

        public override async Task<QueryExecutionResult<ProductQueryResultItem>> Execute()
        {
            var product = _appContext.Products.FirstOrDefault(p => p.Id == Id && !p.IsDeleted);
            if (product == null)
                return await Fail("პროდუქტი ვერ მოიძებნა");

            var result = new ProductQueryResultItem
            {
                Id = product.Id,
                Name = product.Name,
                Annotation = product.Annotation,
                ProductType = product.ProductType,
                ISBN = product.ISBN,
                ReleaseDate = product.ReleaseDate,
                PublisherId = product.PublisherId,
                PageCount = product.PageCount,
                Address = product.Address,
            };

            return await Ok(result);
        }
    }
}
