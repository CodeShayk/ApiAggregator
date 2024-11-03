using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class OrdersApi : WebApi<CollectionResult<OrderResult>>
    {
        public OrdersApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as child to customer api.
            var customer = (CustomerResult)parentApiResult;

            return string.Format(Endpoints.BaseAddress + Endpoints.Orders, customer.Id);
        }
    }
}