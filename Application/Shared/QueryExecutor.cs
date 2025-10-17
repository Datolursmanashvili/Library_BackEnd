using FluentValidation.Results;
using Infrastructure.DB;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Reflection;
using FluentValidation.Attributes;

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
                var validationResult = Validate(query);
                if (!validationResult.IsValid)
                {
                    return new QueryExecutionResult<TResult>
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(error => new Error
                        {
                            Message = error.ErrorMessage,
                            Code = 0
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
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = 0,
                            Message = ex.ToString() // TEMP:
                        }
                    }
                });
            }

        }
        public ValidationResult Validate<T>(Query<T> execution) where T : class
        {
            var validatorAttribute = execution.GetType().GetCustomAttribute<ValidatorAttribute>(true);
            if (validatorAttribute != null)
            {
                var instance = (dynamic)Activator.CreateInstance(validatorAttribute.ValidatorType);
                var modelState = instance.Validate((dynamic)execution);
                return modelState;
            }

            return new ValidationResult();
        }
    }
}

