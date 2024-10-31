using Schemio;
using Schemio.Helpers;

namespace ApiAggregator
{
    public class ColonSeparatedMatcher : ISchemaPathMatcher
    {
        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
              // Does the template xpath contain any of the mapping xpaths?
              inputXPath.IsNotNullOrEmpty()
              && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower()));
    }
}