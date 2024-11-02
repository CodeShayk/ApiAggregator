using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.ResultTransformers
{
    public class CustomerTransform : ResultTransformer<CustomerResult, Customer>
    {
        public override void Transform(CustomerResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Id = queryResult.Id;
            customer.Name = queryResult.Name;
            customer.Code = queryResult.Code;
        }
    }
}