using Application.Shared;
using Domain.Entities.BookAuthorEntity;
using Shared;
using static Application.Queries.BookAuthorQueries.GetAllBookAuthorsQuery;

namespace Application.Queries.BookAuthorQueries;

public class GetAllBookAuthorsQuery : Query<List<BookAuthorQueryResult>>
{
    public override async Task<QueryExecutionResult<List<BookAuthorQueryResult>>> Execute()
    {
        var data = await _bookAuthorRepository.GetAllAsync();

        var result = data?.Select(x => new BookAuthorQueryResult
        {
            Id = x.Id,
            ProductId = x.ProductId,
            AuthorId = x.AuthorId
        }).ToList();

        return await Ok(result);
    }

    public class BookAuthorQueryResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AuthorId { get; set; }
    }
}
