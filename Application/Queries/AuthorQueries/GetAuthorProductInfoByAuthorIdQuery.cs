using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.AuthorQueries;

public class GetAuthorProductInfoByAuthorIdQuery : Query<AuthorProductInfoResult>
{
    public int AuthorId { get; set; }

    public override async Task<QueryExecutionResult<AuthorProductInfoResult>> Execute()
    {
        var author = await _appContext.Authors.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == AuthorId);

        if (author == null) return await Fail(" ავტორი ვერ მოიძებნა");

        var bookAuthor = _appContext.BookAuthors.Where(x => x.IsDeleted == false && x.AuthorId == AuthorId).AsNoTracking();

        var location = _appContext.Locations.Where(x => x.Id == author.CityId || x.Id == author.CountryId)
                                               .AsNoTracking()
                                               .ToDictionary(x => x.Id, x => x.Name);

        var data = new AuthorProductInfoResult
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Gender = author.Gender,
            PersonalNumber = author.PersonalNumber,
            BirthDate = author.BirthDate,
            CountryId = author.CountryId,
            CityId = author.CityId,
            PhoneNumber = author.PhoneNumber,
            Email = author.Email,
            CityName = location.GetValueOrDefault(author.CityId),
            CountryName = location.GetValueOrDefault(author.CityId),
            Products = await (from ba in bookAuthor
                              join p in _appContext.Products
                                  on ba.ProductId equals p.Id
                              where !ba.IsDeleted
                                    && ba.AuthorId == AuthorId
                                    && !p.IsDeleted
                              select new ProductInfoResult
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  ProductType = p.ProductType,
                                  ISBN = p.ISBN,
                                  ReleaseDate = p.ReleaseDate,
                                  PublisherId = p.PublisherId,
                                  PageCount = p.PageCount,
                                  Address = p.Address
                              })
                               .AsNoTracking()
                               .ToListAsync()
        };
        return await Ok(data);
    }
}

public class AuthorProductInfoResult
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string? CountryName { get; set; }
    public string? CityName { get; set; }
    public List<ProductInfoResult>? Products { get; set; }
}

public class ProductInfoResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProductType { get; set; }
    public string ISBN { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PublisherId { get; set; }
    public int PageCount { get; set; }
    public string Address { get; set; }
}
