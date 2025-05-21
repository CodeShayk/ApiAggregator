using ApiAggregator.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ApiAggregator.Impl
{
    internal class ApiExecutor : IApiExecutor
    {
        private readonly IApiEngine apiEngine;

        public ApiExecutor(IApiEngine apiEngine)
        {
            this.apiEngine = apiEngine;
            Constraints.NotNull(apiEngine);
        }

        public IList<IApiResult> Execute(IRequestContext context, IApiList apis)
        {
            context.Cache = new Dictionary<string, object>();
            var globalResults = new List<IApiResult>();
            var counter = 0;

            // supports only 5 levels of api nesting.
            while (apis.As<IWebApi>().Any() && ++counter <= 5)
                apis = Process(context, apis, globalResults);

            return globalResults;
        }

        private IApiList Process(IRequestContext context, IApiList apiList, List<IApiResult> globalResults)
        {
            var subscriber = new EventSubscriber(apiList.GetChildrenApis());
            var eventPublisher = new EventPublisher(subscriber);

            var results = RunApis(apiList);

            CacheResults(context, results);

            eventPublisher.PublishEvent(context, new ExecutorResultArgs(results));

            globalResults.AddRange(results);

            return subscriber.ResolvedDependents;
        }

        private static void CacheResults(IRequestContext context, List<IApiResult> results)
        {
            foreach (var cacheResult in results.Where(result => result.GetType()
                                             .GetCustomAttributes(typeof(CacheResultAttribute), false)
                                             .Any()))
                if (!context.Cache.ContainsKey(cacheResult.GetType().Name))
                    context.Cache.Add(cacheResult.GetType().Name, cacheResult);
        }

        private List<IApiResult> RunApis(IApiList apiList)
        {
            var output = new List<IApiResult>();

            var apis = apiList.Apis.Where(x => apiEngine.CanExecute(x));

            var results = apiEngine.Execute(apis);

            if (results != null)
                output.AddRange(results);

            return output;
        }
    }
}