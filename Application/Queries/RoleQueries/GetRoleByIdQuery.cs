using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Queries.RoleQueries
{
    /// <summary>
    /// Load one role by id (POST body JSON).
    /// </summary>
    public class GetRoleByIdQuery : Query<GetRoleByIdQueryResult>
    {
        /// <summary>Role primary key.</summary>
        public string? Id { get; set; }

        /// <inheritdoc />
        public override async Task<QueryExecutionResult<GetRoleByIdQueryResult>> Execute()
        {
            var ApplicationRoles = ApplicationContext.Set<ApplicationRole>().FirstOrDefault(x => !x.IsDeleted && x.Id == Id);

            if (ApplicationRoles.IsNull()) return await Fail(StatusCodes.Status404NotFound, "ასეთი_როლი_ვერ_მოიძებნა");//ასეთი როლი ვერ მოიძებნა");

            var result = new GetRoleByIdQueryResultItem { Id = ApplicationRoles.Id, Name = ApplicationRoles.Name, Permissions = ApplicationRoles.Permissions };

            return await Ok(new GetRoleByIdQueryResult() { Response = result });
        }
    }

    /// <summary>Wrapper for a single role payload.</summary>
    public class GetRoleByIdQueryResult
    {
        /// <summary>Role details when found.</summary>
        public GetRoleByIdQueryResultItem? Response { get; set; }
    }

    /// <summary>Role detail for Swagger/schema.</summary>
    public class GetRoleByIdQueryResultItem
    {
        /// <summary>Role name.</summary>
        public string? Name { get; set; }

        /// <summary>Role id.</summary>
        public string? Id { get; set; }

        /// <summary>Assigned permission flags.</summary>
        public IEnumerable<Permissions>? Permissions { get; set; }
    }

    /// <summary>FluentValidation for <see cref="GetRoleByIdQuery"/>.</summary>
    public class GetRoleByIdQueryValidation : AbstractValidator<GetRoleByIdQuery>
    {
        /// <summary>Creates the validator.</summary>
        public GetRoleByIdQueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("როლის_იდენტიფიკატორის_მითითება_სავალდებულოა");// "როლის იდენტითიფიკატორის ველის შევსება სავალდებულოა");
        }
    }
}
