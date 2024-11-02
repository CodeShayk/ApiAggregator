using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.WebApis
{
    internal class OrderItemsApi : WebApi<OrderItemResult>
    {
        public OrderItemsApi() : base("http://sys.test.01.net")
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Execute as nested api to order parent api taking OrderResult to resolve api parameter.
            var orders = (CollectionResult<OrderResult>)parentApiResult;
            var customerContext = (CustomerContext)context;

            return $"v2/clients/{customerContext.CustomerId}/orders/items?$filter=orderId in {orders.Select(o => o.OrderId).ToCSV()}";
        }
    }
}