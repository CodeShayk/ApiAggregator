using Microsoft.Extensions.Logging;
using System.Text.Json;
using Schemio;

namespace ApiAggregator
{
    /// <summary>
    /// Implement to create a Web query using api endpoint.
    /// </summary>
    /// <typeparam name="TParameter">Type of Query parameter</typeparam>
    /// <typeparam name="TResult">Type of Query Result</typeparam>
    public abstract class BaseWebQuery<TParameter, TResult> : BaseQuery<TParameter, TResult>, IWebQuery, IRootQuery, IChildQuery
       where TParameter : IQueryParameter where TResult : IQueryResult
    {
        protected BaseWebQuery(string baseAddress)
        {
            BaseAddress = baseAddress;
            Url = GetUrl(QueryParameter);
            Headers = GetHeaders();
        }

        /// <summary>
        /// List of Request headers for the api call.
        /// </summary>
        public List<KeyValuePair<string, string>> Headers { get; protected set; }

        /// <summary>
        /// Base address for the api call.
        /// </summary>
        public string BaseAddress { get; protected set; }

        /// <summary>
        /// Api endpoint - complete or relative.
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Override to pass custom headers with the api request.
        /// </summary>
        /// <returns></returns>
        protected virtual List<KeyValuePair<string, string>>? GetHeaders()
        { return []; }

        /// <summary>
        /// Implement to construct the api endpoint.
        /// </summary>
        /// <param name="queryParameter">Query Parameter</param>
        /// <returns></returns>
        protected abstract string? GetUrl(TParameter queryParameter);

        /// <summary>
        /// Implement to resolve query parameter.
        /// </summary>
        /// <param name="context">root context.</param>
        /// <param name="parentQueryResult">query result from parent query (when configured as nested query). Can be null.</param>
        protected abstract void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult);

        /// <summary>
        /// Implement to resolve query parameter for nested queries
        /// </summary>
        /// <param name="context">root context</param>
        /// <param name="parentQueryResult">query result from parent query.</param>
        public void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            ResolveQueryParameter(context, parentQueryResult);
        }

        /// <summary>
        /// Implement to resolve query parameter for first level queries.
        /// </summary>
        /// <param name="context">root context</param>
        public void ResolveRootQueryParameter(IDataContext context)
        {
            ResolveQueryParameter(context, null);
        }

        /// <summary>
        /// Run this web query to get results.
        /// </summary>
        /// <param name="httpClientFactory">HttpClientFactory</param>
        /// <param name="logger">Logger</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">when httpclientfactory is null.</exception>
        public virtual async Task<IQueryResult[]> Run(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException("HttpClientFactory is required");

            var localStorage = new List<TResult>();

            logger?.LogInformation($"Run query: {GetType().Name}");

            using (var client = httpClientFactory.CreateClient())
            {
                logger?.LogInformation($"Executing web queries on thread {Thread.CurrentThread.ManagedThreadId} (task {Task.CurrentId})");

                try
                {
                    HttpResponseMessage result;

                    try
                    {
                        if (!string.IsNullOrEmpty(BaseAddress))
                            client.BaseAddress = new Uri(BaseAddress);

                        if (Headers != null && Headers.Any())
                            foreach (var header in Headers)
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);

                        result = await client.GetAsync(Url);

                        if (!result.IsSuccessStatusCode)
                        {
                            logger?.LogInformation($"Result of executing web query {Url} is not success status code");
                        }

                        var raw = result.Content.ReadAsStringAsync().Result;

                        if (string.IsNullOrWhiteSpace(raw))
                            logger?.LogInformation($"Result.Content of executing web query {Url} is null or whitespace");

                        if (ResultType.IsArray)
                        {
                            var arrObject = JsonSerializer.Deserialize(raw, ResultType);
                            if (arrObject != null)
                                localStorage.AddRange((IEnumerable<TResult>)arrObject);
                        }
                        else
                        {
                            var obj = JsonSerializer.Deserialize(raw, ResultType);
                            if (obj != null)
                                localStorage.Add((TResult)obj);
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        logger?.LogWarning(ex, $"An error occurred while sending the request. Query URL: {Url}");
                    }
                    catch (HttpRequestException ex)
                    {
                        logger?.LogWarning(ex, $"An error occurred while sending the request. Query URL: {Url}");
                    }
                }
                catch (AggregateException ex)
                {
                    logger?.LogInformation($"Web query {GetType().Name} failed");
                    foreach (var e in ex.InnerExceptions)
                    {
                        logger?.LogError(e, "");
                    }
                }
            }

            return localStorage.Cast<IQueryResult>().ToArray();
        }
    }
}