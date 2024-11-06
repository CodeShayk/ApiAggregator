namespace ApiAggregator
{
    public interface IRequestContext : IApiResultCache
    {
        /// <summary>
        /// Aggregate api names for data retrieval.
        /// </summary>
        public string[] Names { get; }
    }
}