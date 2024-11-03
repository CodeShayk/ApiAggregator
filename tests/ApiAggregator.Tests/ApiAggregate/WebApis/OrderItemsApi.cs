using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    internal class OrderItemsApi : WebApi<OrderItemResult>
    {
        public OrderItemsApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as nested api to order parent api taking OrderResult to resolve api parameter.
            var orders = (CollectionResult<OrderResult>)parentApiResult;
            var customerContext = (CustomerContext)context;

            return string.Format(Endpoints.BaseAddress + Endpoints.OrderItems, customerContext.CustomerId, orders.Select(o => o.OrderId).ToCSV());
        }
    }
}