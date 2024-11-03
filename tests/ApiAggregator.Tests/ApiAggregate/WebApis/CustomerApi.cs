using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    public class CustomerApi : WebApi<CustomerResult>
    {
        public CustomerApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Executes as root or level 1 api.
            var customerContext = (CustomerContext)context;

            return string.Format(Endpoints.BaseAddress + Endpoints.Customer, customerContext.CustomerId);
        }
    }
}