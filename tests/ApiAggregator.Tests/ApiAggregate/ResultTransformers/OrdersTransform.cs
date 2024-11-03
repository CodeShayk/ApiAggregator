using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.ApiAggregate.ApiResults;
using static ApiAggregator.Tests.ApiAggregate.Customer;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class OrdersTransform : ResultTransformer<OrderResult, Customer>
    {
        public override void Transform(OrderResult apiResult, Customer contract)
        {
            if (apiResult == null)
                return;

            var customer = contract ?? new Customer();

            customer.Orders = ArrayUtil.EnsureAndResizeArray(customer.Orders, out var index);

            customer.Orders[index] = new Order
            {
                Date = apiResult.Date,
                OrderId = apiResult.OrderId,
                OrderNo = apiResult.OrderNo
            };
        }
    }
}