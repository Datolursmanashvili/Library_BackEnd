using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Queries.RoleQueries
{
    [Validator(typeof(GetRoleByIdQueryValidation))]
    public class GetRoleByIdQuery : Query<GetRoleByIdQueryResult>
    {
        public string? Id { get; set; }
        public override async Task<QueryExecutionResult<GetRoleByIdQueryResult>> Execute()
        {
            var ApplicationRoles = ApplicationContext.Set<ApplicationRole>().FirstOrDefault(x => !x.IsDeleted && x.Id == Id);

            if (ApplicationRoles.IsNull()) return await Fail("ასეთი_როლი_ვერ_მოიძებნა");//ასეთი როლი ვერ მოიძებნა");

            var result = new GetRoleByIdQueryResultItem { Id = ApplicationRoles.Id, Name = ApplicationRoles.Name, Permissions = ApplicationRoles.Permissions };

            return await Ok(new GetRoleByIdQueryResult() { Response = result });
        }
    }

    public class GetRoleByIdQueryResult
    {
        public GetRoleByIdQueryResultItem? Response { get; set; }
    }
    public class GetRoleByIdQueryResultItem
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public IEnumerable<Permissions>? Permissions { get; set; }
    }
    public class GetRoleByIdQueryValidation : AbstractValidator<GetRoleByIdQuery>
    {
        public GetRoleByIdQueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("როლის_იდენტიფიკატორის_მითითება_სავალდებულოა");// "როლის იდენტითიფიკატორის ველის შევსება სავალდებულოა");
        }
    }
}
