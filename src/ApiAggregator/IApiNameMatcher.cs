namespace ApiAggregator.Net
{
    public interface IApiNameMatcher
    {
        bool IsMatch(string inputPath, IApiNames configuredPaths);
    }
}