namespace ApiAggregator.Net
{
    /// <summary>
    /// Implement to get supported Api result.
    /// </summary>
    public interface ITransformerResultType
    {
        /// <summary>
        /// Supported api reslt.
        /// </summary>
        Type SupportedApiResult { get; }
    }
}