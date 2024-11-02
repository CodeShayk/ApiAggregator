
namespace ApiAggregator.Net
{
    public abstract class RequestContext : IRequestContext
    {
        public string[] Names { get; set; }
        Dictionary<string, object> IApiResultCache.Cache { get; set; }
    }
}