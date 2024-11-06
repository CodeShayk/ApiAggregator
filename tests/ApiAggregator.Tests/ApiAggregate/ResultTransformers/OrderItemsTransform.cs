using System.Linq;
using System.Xml.Linq;
using ApiAggregator.Helpers;
using ApiAggregator.Tests.ApiAggregate.ApiResults;
using Microsoft.Extensions.Hosting;
using static ApiAggregator.Tests.ApiAggregate.Customer.Order;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class OrderItemsTransform : ResultTransformer<CollectionResult<OrderItemResult>, Customer>
    {
        public override void Transform(CollectionResult<OrderItemResult> collectionResult, Customer customer)
        {
            if (collectionResult == null || !collectionResult.Any() || customer.Orders == null)
                return;

            foreach (var result in collectionResult)
            {
                var order = customer.Orders.FirstOrDefault(o => o.OrderId == result.OrderId);
                if (order == null)
                    continue;

                order.Items = ArrayUtil.EnsureAndResizeArray(order.Items, out var index);
                order.Items[index] = new OrderItem
                {
                    ItemId = result.ItemId,
                    Name = result.Name,
                    Cost = result.Cost
                };
            }
        }
    }
}