using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.WebApis
{
    public class CustomerApi : WebApi<CustomerResult>
    {
        public CustomerApi() : base("http://sys.test.01.net")
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Executes as root or level 1 api.
            var customerContext = (CustomerContext)context;

            return $"v2/clients/{customerContext.CustomerId}";
        }
    }
}