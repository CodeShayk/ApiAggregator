using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class CommunicationApi : WebApi<CommunicationResult>
    {
        public CommunicationApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override Uri GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            var customer = (CustomerResult)parentApiResult;
            return new Uri(string.Format(Endpoints.BaseAddress + Endpoints.Communication, customer.Id), UriKind.Absolute);
        }
    }
}