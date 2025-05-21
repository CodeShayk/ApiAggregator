using System.Collections.Generic;

namespace ApiAggregator
{
    public interface IApiResultCache
    {
        /// <summary>
        /// Cache dictionary holding api results for api result type marked with [CacheResult] attribute.
        /// </summary>
        Dictionary<string, object> Cache { get; set; }
    }
}