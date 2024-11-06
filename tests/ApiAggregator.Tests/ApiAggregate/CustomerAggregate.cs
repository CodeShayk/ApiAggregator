using ApiAggregator.Tests.ApiAggregate.ResultTransformers;
using ApiAggregator.Tests.ApiAggregate.WebApis;

namespace ApiAggregator.Tests.ApiAggregate
{
    internal class CustomerAggregate : ApiAggregate<Customer>
    {
        /// <summary>
        /// Constructs the api aggregate with web apis and result transformers to map data to aggregated contract.
        /// </summary>
        /// <returns>Mappings</returns>
        public override IEnumerable<Mapping<Customer, IApiResult>> Construct()
        {
            return CreateAggregate.For<Customer>()
                .Map<CustomerApi, CustomerTransform>(With.Name("customer"),
                 customer => customer.Dependents
                    .Map<CommunicationApi, CommunicationTransform>(With.Name("customer.communication"))
                    .Map<OrdersApi, OrdersTransform>(With.Name("customer.orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<OrderItemsApi, OrderItemsTransform>(With.Name("customer.orders.items")))
                ).Create();
        }
    }
}