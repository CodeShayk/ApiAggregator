namespace ApiAggregator.Net
{
    public interface IApiResult
    {
        List<KeyValuePair<string, string>> Headers { get; }
    }
}