namespace ApiAggregator.Net
{
    /// <summary>
    /// Implement transformer to map data from supported api result to aggregated contract in context.
    /// </summary>
    public interface IResultTransformer
    {
        /// <summary>
        /// Transform method to map data to aggreated contract for a given api result.
        /// </summary>
        /// <param name="apiResult">Supported Api Result.</param>
        /// <param name="contract">Aggregate Contract.</param>
        void Transform(IApiResult apiResult, IContract contract);
    }
}