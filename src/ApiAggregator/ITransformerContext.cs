namespace ApiAggregator
{
    /// <summary>
    /// Implement to set transform with request.
    /// </summary>
    public interface ITransformerContext
    {
        /// <summary>
        /// Implement to set request context in transform.
        /// </summary>
        /// <param name="context">Request Context</param>
        void SetContext(IRequestContext context);
    }
}