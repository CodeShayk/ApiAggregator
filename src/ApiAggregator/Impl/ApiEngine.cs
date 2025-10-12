using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
                return Enumerable.Empty<IApiResult>();

            logger?.LogInformation($"Total web apis to execute: {apis.Count()}");

            var tasks = apis
                .Select(q => q.Run(httpClientFactory, logger))
                .ToArray();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ex)
            {
                // Re-throw the actual exception that occurred in the tasks
                throw ex.Flatten();
            }

            var results = new List<IApiResult>();

            for (int i = 0; i < tasks.Length; i++)
            {
                var result = tasks[i].Result;
                if (result != null)
                    results.Add(result);
            }

            return results.ToArray();
        }
    }
}