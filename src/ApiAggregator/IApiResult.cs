namespace ApiAggregator
{
    public interface IApiResult
    {
        IDictionary<string, string> Headers { get; internal set; }
    }
}