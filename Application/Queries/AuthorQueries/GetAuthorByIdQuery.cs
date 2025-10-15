using Application.Shared;
using Shared;

namespace Application.Queries.AuthorQueries
{
    public class GetAuthorByIdQuery : Query<AuthorQueryResultItem>
    {
        public int Id { get; set; }

        public override async Task<QueryExecutionResult<AuthorQueryResultItem>> Execute()
        {
            var author = _appContext.Authors.FirstOrDefault(a => a.Id == Id && !a.IsDeleted);
            if (author == null)
                return await Fail("ავტორი ვერ მოიძებნა");

            var result = new AuthorQueryResultItem
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
                Email = author.Email
            };

            return await Ok(result);
        }
    }
}
