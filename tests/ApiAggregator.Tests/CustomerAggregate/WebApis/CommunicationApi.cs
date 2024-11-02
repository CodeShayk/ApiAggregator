using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.WebApis
{
    internal class CommunicationApi : WebApi<CommunicationResult>
    {
        public CommunicationApi() : base("http://sys.test.01.net")
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            var customer = (CustomerResult)parentApiResult;
            return $"v2/clients/{customer.Id}/communication";
        }
    }
}