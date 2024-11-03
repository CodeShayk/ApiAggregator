using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class CustomerTransform : ResultTransformer<CustomerResult, Customer>
    {
        public override void Transform(CustomerResult apiResult, Customer contract)
        {
            var customer = contract ?? new Customer();
            customer.Id = apiResult.Id;
            customer.Name = apiResult.Name;
            customer.Code = apiResult.Code;
        }
    }
}