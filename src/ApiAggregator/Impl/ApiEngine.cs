using ApiAggregator.Helpers;
using Microsoft.Extensions.Logging;

namespace ApiAggregator.Impl
{
    internal class ApiEngine : IApiEngine
    {
        private readonly ILogger<ApiEngine> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public ApiEngine(IHttpClientFactory httpClientFactory, ILogger<ApiEngine> logger = null)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;

            Constraints.NotNull(httpClientFactory);
        }

        public bool CanExecute(IWebApi api) => api is IWebApi;

        public IEnumerable<IApiResult> Execute(IEnumerable<IWebApi> apis)
        {
            if (apis == null || !apis.Any())
                return [];

            logger?.LogInformation($"Total web apis to execute: {apis.Count()}");

            var tasks = apis
                .Select(q => q.Run(httpClientFactory, logger))
                .ToArray();

            Task.WhenAll(tasks);

            var results = new List<IApiResult>();

            foreach (var task in tasks)
            {
                var result = task.Result;
                if (result != null)
                    results.Add(result);
            }

            return results.ToArray();
        }
    }
}