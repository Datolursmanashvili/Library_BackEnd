namespace Shared
{
    public class QueryExecutionResult<T>
    {
        public bool Success { get; set; }

        public IEnumerable<Error> Errors { get; set; }

        public T Data { get; set; }
    }
}
