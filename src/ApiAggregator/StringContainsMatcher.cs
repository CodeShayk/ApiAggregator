using ApiAggregator.Helpers;

namespace ApiAggregator
{
    public class StringContainsMatcher : IApiNameMatcher
    {
        public bool IsMatch(string inputName, IApiNames configuredNames) =>
              // Does the input name contain any of the configured names?
              inputName.IsNotNullOrEmpty()
              && configuredNames.Names.Any(x => inputName.ToLower().Contains(x.ToLower()));
    }
}