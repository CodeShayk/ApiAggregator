using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApiAggregator.Net
{
    /// <summary>
    /// Implement to create a Web api instance.
    /// </summary>
    /// <typeparam name="TResult">Type of Api Result</typeparam>
    public abstract class WebApi<TResult> : IWebApi
           where TResult : IApiResult
    {
        protected string BaseAddress;
        protected string Url;
        private bool isContextResolved;

        protected WebApi() : this(null)
        {
        }

        protected WebApi(string baseAddress)
        {
            BaseAddress = baseAddress;
        }

        /// <summary>
        /// Children apis dependent on this api.
        /// </summary>
        List<IWebApi> IWebApi.Children { get; set; }

        /// <summary>
        /// Get the result type for the api
        /// </summary>
        Type IWebApi.ResultType
        {
            get { return typeof(TResult); }
        }

        /// <summary>
        /// Determines whether the api endpoind is resolved.
        /// </summary>
        /// <returns></returns>
        bool IWebApi.IsContextResolved() => isContextResolved;

        /// <summary>
        /// Override to pass custom headers with the api request.
        /// </summary>
        /// <returns></returns>
        protected virtual List<KeyValuePair<string, string>> GetHeaders()
        { return []; }

        /// <summary>
        /// Implement to construct the api endpoint.
        /// </summary>
        /// <param name="context">Request Context. Always available.</param>
        /// <param name="parentApiResult">Result from parent Api. Only available when configured as nested web api. Else will be null.</param>
        /// <returns></returns>
        protected abstract string GetUrl(IRequestContext context, IApiResult parentApiResult = null);

        /// <summary>
        /// Implement to resolve api parameter.
        /// </summary>
        /// <param name="context">Request context.</param>
        /// <param name="parentApiResult">api result from parent api (when configured as nested api). Can be null.</param>
        void IWebApi.ResolveApiParameter(IRequestContext context, IApiResult parentApiResult = null)
        {
            Url = GetUrl(context, parentApiResult);
            isContextResolved = true;
        }

        /// <summary>
        /// Run this web api to get results.
        /// </summary>
        /// <param name="httpClientFactory">HttpClientFactory</param>
        /// <param name="logger">Logger</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">when httpclientfactory is null.</exception>
        public virtual async Task<IApiResult> Run(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException("HttpClientFactory is required");

            logger?.LogInformation($"Run api: {GetType().Name}");

            if (string.IsNullOrEmpty(Url))
                return null;

            using (var client = httpClientFactory.CreateClient())
            {
                logger?.LogInformation($"Executing web api on thread {Thread.CurrentThread.ManagedThreadId} (task {Task.CurrentId})");

                try
                {
                    HttpResponseMessage result;

                    try
                    {
                        if (!string.IsNullOrEmpty(BaseAddress))
                            client.BaseAddress = new Uri(BaseAddress);

                        var headers = GetHeaders();

                        if (headers != null && headers.Any())
                            foreach (var header in headers)
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);

                        result = await client.GetAsync(Url);

                        if (!result.IsSuccessStatusCode)
                            logger?.LogInformation($"Result of executing web api {Url} is not success status code");

                        var raw = result.Content.ReadAsStringAsync().Result;

                        if (string.IsNullOrWhiteSpace(raw))
                            logger?.LogInformation($"Result.Content of executing web api {Url} is null or whitespace");

                        if (typeof(TResult).IsArray)
                        {
                            var arrObject = JsonSerializer.Deserialize(raw, typeof(TResult));
                            if (arrObject != null)
                                return new CollectionResult<TResult>((IEnumerable<TResult>)arrObject);
                        }
                        else
                        {
                            var obj = JsonSerializer.Deserialize(raw, typeof(TResult));
                            if (obj != null)
                                return (TResult)obj;
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
                    logger?.LogInformation($"Web api {GetType().Name} failed");
                    foreach (var e in ex.InnerExceptions)
                        logger?.LogError(e, "");
                }
            }

            return null;
        }
    }
}