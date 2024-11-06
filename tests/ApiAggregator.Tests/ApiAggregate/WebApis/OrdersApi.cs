using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class OrdersApi : WebApi<CollectionResult<OrderResult>>
    {
        public OrdersApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override Uri GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as child to customer api.
            var customer = (CustomerResult)parentApiResult;

            return new Uri(string.Format(Endpoints.BaseAddress + Endpoints.Orders, customer.Id), UriKind.Absolute);
        }
    }
}