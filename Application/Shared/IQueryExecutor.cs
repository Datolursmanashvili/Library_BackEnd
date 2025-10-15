using Shared;
using System.Threading.Tasks;

namespace Application.Shared
{
    public interface IQueryExecutor
    {
        Task<QueryExecutionResult<TResult>> Execute<TQuery, TResult>(TQuery query) where TQuery : Query<TResult> where TResult : class;
    }
}
