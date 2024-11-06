namespace ApiAggregator.Impl
{
    internal class EventSubscriber : ISubscriber<ExecutorResultArgs>
    {
        private readonly IList<NestedApiList> nestedApis;

        public EventSubscriber(IList<NestedApiList> nestedApis)
        {
            this.nestedApis = GetUniqueApiList(nestedApis);
        }

        public void OnEventHandler(IRequestContext context, ExecutorResultArgs args)
        {
            if (args.Result == null)
                return;

            var results = args.Result;

            foreach (var apiResult in results.Where(x => !typeof(IPolymorphicResult).IsAssignableFrom(x.GetType())))
            {
                if (nestedApis.All(tuple => tuple.ParentApiResultType != apiResult.GetType()))
                    continue;

                var result = apiResult;

                foreach (var unresolved in nestedApis)
                {
                    if (unresolved.ParentApiResultType != result.GetType())
                        continue;

                    foreach (var api in unresolved.Apis)
                        api.ResolveApiParameter(context, apiResult);
                }
            }

            foreach (var apiResult in results.Where(x => typeof(IPolymorphicResult).IsAssignableFrom(x.GetType())))
            {
                if (nestedApis.All(tuple => tuple.ParentApiResultType != apiResult.GetType() &&
                    !tuple.ParentApiResultType.IsAssignableFrom(apiResult.GetType()))
                )
                    continue;

                var result = apiResult;

                foreach (var unresolved in nestedApis)
                {
                    if (unresolved.ParentApiResultType != result.GetType() &&
                        !unresolved.ParentApiResultType.IsAssignableFrom(result.GetType()))
                        continue;

                    foreach (var api in unresolved.Apis)
                        api.ResolveApiParameter(context, apiResult);
                }
            }
        }

        public ApiList ResolvedDependents => new ApiList(nestedApis
                .SelectMany(x => x.Apis)
                .Where(api => api.IsContextResolved()));

        private IList<NestedApiList> GetUniqueApiList(IList<NestedApiList> nestedApis)
        {
            if (nestedApis == null)
                return new List<NestedApiList>();

            var distincts = nestedApis
                .Select(x => x.ParentApiResultType)
                .Distinct()
                .Select(x =>
                {
                    var distinctMerge = nestedApis
                        .Where(d => d.ParentApiResultType == x)
                        .SelectMany(q => q.Apis)
                        .Distinct(new ApiComparer())
                        .ToList();

                    return new NestedApiList { ParentApiResultType = x, Apis = distinctMerge };
                })
                .ToList();

            return distincts;
        }
    }
}