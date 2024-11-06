using System;
using ApiAggregator.Tests.ApiAggregate.ApiResults;
using static ApiAggregator.Tests.ApiAggregate.Customer;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class OrdersTransform : ResultTransformer<CollectionResult<OrderResult>, Customer>
    {
        public override void Transform(CollectionResult<OrderResult> collectionResult, Customer contract)
        {
            if (collectionResult == null || !collectionResult.Any())
                return;

            var customer = contract ?? new Customer();

            customer.Orders = new Order[collectionResult.Count];

            for (var index = 0; index < collectionResult.Count; index++)
            {
                customer.Orders[index] = new Order
                {
                    Date = collectionResult[index].Date,
                    OrderId = collectionResult[index].OrderId,
                    OrderNo = collectionResult[index].OrderNo
                };
            }
        }
    }
}