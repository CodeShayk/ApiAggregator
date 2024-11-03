using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class CommunicationApi : WebApi<CommunicationResult>
    {
        public CommunicationApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            var customer = (CustomerResult)parentApiResult;
            return string.Format(Endpoints.BaseAddress + Endpoints.Communication, customer.Id);
        }
    }
}