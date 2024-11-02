using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;
using static ApiAggregator.Tests.CustomerAggregate.AggregateContract.Customer;

namespace ApiAggregator.Tests.CustomerAggregate.ResultTransformers
{
    public class OrdersTransform : ResultTransformer<OrderResult, Customer>
    {
        public override void Transform(OrderResult queryResult, Customer entity)
        {
            if (queryResult == null)
                return;

            var customer = entity ?? new Customer();

            customer.Orders = ArrayUtil.EnsureAndResizeArray(customer.Orders, out var index);

            customer.Orders[index] = new Order
            {
                Date = queryResult.Date,
                OrderId = queryResult.OrderId,
                OrderNo = queryResult.OrderNo
            };
        }
    }
}