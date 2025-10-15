using Application.Shared;
using Shared;

namespace Application.Queries.ProductQueries
{
    public class GetAllProductsQuery : Query<GetAllProductsQueryResult>
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 0;

        public override async Task<QueryExecutionResult<GetAllProductsQueryResult>> Execute()
        {
            var products = _appContext.Products.Where(p => !p.IsDeleted).AsQueryable();
            var totalCount = products.Count();

            var result = products
                .OrderBy(p => p.Id)
                .Skip(Page * PageSize)
                .Take(PageSize)
                .Select(p => new ProductQueryResultItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Annotation = p.Annotation,
                    ProductType = p.ProductType,
                    ISBN = p.ISBN,
                    ReleaseDate = p.ReleaseDate,
                    PublisherId = p.PublisherId
                })
                .ToList();

            var response = new GetAllProductsQueryResult
            {
                Result = result,
                TotalCount = totalCount
            };

            return await Ok(response);
        }
    }

    public class ProductQueryResultItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Annotation { get; set; }
        public string ProductType { get; set; }
        public string ISBN { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int PublisherId { get; set; }
    }

    public class GetAllProductsQueryResult
    {
        public List<ProductQueryResultItem>? Result { get; set; }
        public int TotalCount { get; set; }
    }
}
