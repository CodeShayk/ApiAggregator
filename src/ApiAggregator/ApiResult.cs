namespace ApiAggregator.Net
{
    public abstract class ApiResult : IApiResult
    {
        public ApiResult()
        {
            Headers = [];
        }

        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}