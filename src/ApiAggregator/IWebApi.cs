using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApiAggregator
{
    /// <summary>
    /// Implement IWebApi to fetch data using API.
    /// </summary>
    public interface IWebApi
    {
        List<IWebApi> Children { get; set; }

        Type ResultType { get; }

        bool IsContextResolved();

        Task<IApiResult> Run(IHttpClientFactory httpClientFactory, ILogger logger = null);

        /// <summary>
        /// Implement to resolve api parameter.
        /// </summary>
        /// <param name="context">Request Context</param>
        /// <param name="parentApiResult">Result from parent Api in nested mode.</param>
        void ResolveApiParameter(IRequestContext context, IApiResult parentApiResult = null);
    }
}