namespace ApiAggregator.Net.Impl
{
    internal class ApiBuilder<T> : IApiBuilder<T> where T : IContract
    {
        private readonly IApiAggregate<T> apiAggregate;
        private readonly IApiNameMatcher apiNameMatcher;

        public ApiBuilder(IApiAggregate<T> apiAggregate, IApiNameMatcher apiNameMatcher)
        {
            this.apiAggregate = apiAggregate;
            this.apiNameMatcher = apiNameMatcher;
        }

        /// <summary>
        /// Builds a list of apis for the aggregated contract in context based on the api names provided.
        /// </summary>
        /// <param name="context">Request Context</param>
        /// <returns></returns>
        public IApiList Build(IRequestContext context)
        {
            var apiList = GetMappedApis(apiAggregate.Mappings.ToList(), context);

            foreach (var api in apiList.Apis)
                api.ResolveApiParameter(context);

            return new ApiList(apiList.Apis);
        }

        private ApiList GetMappedApis(IReadOnlyCollection<Mapping<T, IApiResult>> mappings, IRequestContext context)
        {
            var apiNestingDepth = mappings.Max(x => x.Order);

            for (var index = 1; index <= apiNestingDepth; index++)
            {
                var maps = mappings.Where(x => x.Order == index);

                foreach (var map in maps)
                {
                    var dependentApis =
                        mappings.Where(x => x.Order == index + 1 && x.DependentOn != null && x.DependentOn.GetType() == map.Api.GetType()).ToList();

                    map.Api.Children ??= new List<IWebApi>();

                    map.Api.Children.AddRange(FilterByPaths(context, dependentApis));
                }
            }

            var apis = FilterByPaths(context, mappings.Where(x => x.DependentOn == null))
                .Distinct(new ApiComparer())
                .ToList();

            return new ApiList(apis) { ApiNestingDepth = apiNestingDepth };
        }

        private IEnumerable<IWebApi> FilterByPaths(IRequestContext context, IEnumerable<Mapping<T, IApiResult>> mappings)
        {
            var matchedMappings = context.Names != null
                ? mappings.Where(mapping => context.Names.Any(Path => apiNameMatcher.IsMatch(Path, mapping.SchemaPaths)))
                .ToList()
                : mappings;

            return matchedMappings
            .Select(x => x.Api)
                .Distinct(new ApiComparer());
        }
    }
}