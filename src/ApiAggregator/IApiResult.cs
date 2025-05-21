using System.Collections.Generic;

namespace ApiAggregator
{
    public interface IApiResult
    {
        IDictionary<string, string> Headers { get; set; }
    }
}