using FluentValidation;
using FluentValidation.Results;
using Infrastructure.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Application.Shared
{
    public class QueryExecutor : IQueryExecutor
    {
        private ApplicationDbContext _appContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;


        public QueryExecutor(IServiceProvider serviceProvider,
            IConfiguration configuration,
            ApplicationDbContext appContext
           )
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _appContext = appContext;
        }

        public async Task<QueryExecutionResult<TResult>> Execute<TQuery, TResult>(TQuery query)
            where TQuery : Query<TResult>
            where TResult : class
        {
            try
            {
                var validationResult = Validate<TQuery, TResult>(query);
                if (!validationResult.IsValid)
                {
                    return new QueryExecutionResult<TResult>
                    {
                        Success = false,
                        HttpStatusCode = StatusCodes.Status400BadRequest,
                        Errors = validationResult.Errors.Select(error => new Error
                        {
                            Message = error.ErrorMessage,
                            Code = StatusCodes.Status400BadRequest
                        })
                    };
                }

                query.Resolve(
                   _appContext,
                   _serviceProvider);

                return await query.Execute();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new QueryExecutionResult<TResult>
                {
                    Success = false,
                    HttpStatusCode = StatusCodes.Status500InternalServerError,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status500InternalServerError,
                            Message = ex.ToString()
                        }
                    }
                });
            }

        }
        public ValidationResult Validate<TQuery, TResult>(TQuery execution)
            where TQuery : Query<TResult>
            where TResult : class
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(execution.GetType());
            var validator = (IValidator?)_serviceProvider.GetService(validatorType);

            if (validator != null)
            {
                return validator.Validate(new ValidationContext<object>(execution));
            }

            return new ValidationResult();
        }
    }
}

