using Microsoft.Extensions.Logging;
using Schemio;

namespace ApiAggregator
{
    public interface IWebQuery : IQuery
    {
        List<KeyValuePair<string, string>> Headers { get; }
        string BaseAddress { get; }
        string Url { get; }

        Task<IQueryResult[]> Run(IHttpClientFactory httpClientFactory, ILogger logger);
    }
}