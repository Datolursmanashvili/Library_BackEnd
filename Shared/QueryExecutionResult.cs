namespace Shared
{
    /// <summary>
    /// Standard envelope for query (read) operations. HTTP status on the wire matches <see cref="HttpStatusCode"/> property when returned via API helpers.
    /// </summary>
    /// <typeparam name="T">Payload type returned in <see cref="Data"/> on success.</typeparam>
    public class QueryExecutionResult<T>
    {
        /// <summary>True when the operation completed without business/validation errors.</summary>
        public bool Success { get; set; }

        /// <summary>Suggested HTTP status for this result (e.g. 200, 400, 401, 404).</summary>
        public int HttpStatusCode { get; set; } = 200;

        /// <summary>Validation or business error messages when <see cref="Success"/> is false.</summary>
        public IEnumerable<Error> Errors { get; set; }

        /// <summary>Result payload when <see cref="Success"/> is true.</summary>
        public T Data { get; set; }
    }
}
