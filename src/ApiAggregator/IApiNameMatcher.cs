namespace ApiAggregator
{
    public interface IApiNameMatcher
    {
        bool IsMatch(string inputPath, IApiNames configuredPaths);
    }
}