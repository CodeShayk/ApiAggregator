using System.Collections.Generic;

namespace ApiAggregator
{
    public abstract class ApiResult : IApiResult
    {
        public ApiResult()
        {
            Headers = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Headers { get; set; }
    }
}