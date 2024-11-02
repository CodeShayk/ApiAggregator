using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;
using static ApiAggregator.Tests.CustomerAggregate.AggregateContract.Customer.Order;

namespace ApiAggregator.Tests.CustomerAggregate.ResultTransformers
{
    public class OrderItemsTransform : ResultTransformer<OrderItemResult, Customer>
    {
        public override void Transform(OrderItemResult queryResult, Customer entity)
        {
            if (queryResult == null || entity?.Orders == null)
                return;

            foreach (var order in entity.Orders)
                if (order.OrderId == queryResult.OrderId)
                {
                    order.Items = ArrayUtil.EnsureAndResizeArray(order.Items, out var index);
                    order.Items[index] = new OrderItem
                    {
                        ItemId = queryResult.ItemId,
                        Name = queryResult.Name,
                        Cost = queryResult.Cost
                    };
                }
        }
    }
}