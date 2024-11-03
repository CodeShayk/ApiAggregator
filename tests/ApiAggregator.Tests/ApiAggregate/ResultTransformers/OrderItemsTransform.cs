using ApiAggregator.Net;
using ApiAggregator.Net.Helpers;
using ApiAggregator.Tests.ApiAggregate.ApiResults;
using static ApiAggregator.Tests.ApiAggregate.Customer.Order;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class OrderItemsTransform : ResultTransformer<OrderItemResult, Customer>
    {
        public override void Transform(OrderItemResult apiResult, Customer contract)
        {
            if (apiResult == null || contract?.Orders == null)
                return;

            foreach (var order in contract.Orders)
                if (order.OrderId == apiResult.OrderId)
                {
                    order.Items = ArrayUtil.EnsureAndResizeArray(order.Items, out var index);
                    order.Items[index] = new OrderItem
                    {
                        ItemId = apiResult.ItemId,
                        Name = apiResult.Name,
                        Cost = apiResult.Cost
                    };
                }
        }
    }
}