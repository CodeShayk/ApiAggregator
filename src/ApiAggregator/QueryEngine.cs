using Microsoft.Extensions.Logging;
using Schemio;

namespace ApiAggregator
{
    public class QueryEngine : IQueryEngine
    {
        private readonly ILogger<QueryEngine> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public QueryEngine(IHttpClientFactory httpClientFactory, ILogger<QueryEngine> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public bool CanExecute(IQuery query) => query is IWebQuery;

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries)
        {
            if (queries == null || !queries.Any())
                return [];

            var webQueries = queries.GetByType<IWebQuery>();

            if (!webQueries.Any())
                return [];

            logger.LogInformation($"Total web queries to execute: {webQueries.Count()}");

            var tasks = webQueries
                .Select(q => q.Run(httpClientFactory, logger))
                .ToArray();

            Task.WhenAll(tasks);

            var result = new List<IQueryResult>();

            foreach (var task in tasks)
            {
                result.AddRange(task.Result);
            }

            return result.ToArray();
        }
    }
}