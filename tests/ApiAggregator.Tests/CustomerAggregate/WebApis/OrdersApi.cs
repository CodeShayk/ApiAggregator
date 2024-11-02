using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.WebApis
{
    internal class OrdersApi : WebApi<CollectionResult<OrderResult>>
    {
        public OrdersApi() : base("http://sys.test.01.net")
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as child to customer api.
            var customer = (CustomerResult)parentApiResult;

            return $"v2/clients/{customer.Id}/orders";
        }
    }
}