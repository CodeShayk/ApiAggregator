namespace ApiAggregator.Net
{
    public interface IApiResultCache
    {
        /// <summary>
        /// Cache dictionary holding api results for api result type marked with [CacheResult] attribute.
        /// </summary>
        internal Dictionary<string, object> Cache { get; set; }
    }
}