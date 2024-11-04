namespace ApiAggregator.Net
{
    public class CollectionResult<T> : List<T>, IApiResult
    {
        public CollectionResult(IEnumerable<T> list) : base(list)
        {
            Headers = [];
        }

        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}