using ApiAggregator.Helpers;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class OrderItemsApi : WebApi<CollectionResult<OrderItemResult>>
    {
        public OrderItemsApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override Uri GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as nested api to order parent api taking OrderResult to resolve api parameter.
            var orders = (CollectionResult<OrderResult>)parentApiResult;
            var customerContext = (CustomerContext)context;

            return new Uri(string.Format(Endpoints.BaseAddress + Endpoints.OrderItems, customerContext.CustomerId, orders.Select(o => o.OrderId).ToCSV()), UriKind.Absolute);
        }
    }
}