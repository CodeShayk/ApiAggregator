using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using ApiAggregator.Tests.CustomerAggregate.ResultTransformers;
using ApiAggregator.Tests.CustomerAggregate.WebApis;

namespace ApiAggregator.Tests.CustomerAggregate
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