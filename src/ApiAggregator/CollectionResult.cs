namespace ApiAggregator
{
    public class CollectionResult<T> : List<T>, IApiResult
    {
        public CollectionResult(IEnumerable<T> list) : base(list)
        {
            Headers = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Headers { get; set; }
    }
}