using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using ApiAggregator.Tests.CustomerAggregate.ApiResults;

namespace ApiAggregator.Tests.CustomerAggregate.ResultTransformers
{
    public class CommunicationTransform : ResultTransformer<CommunicationResult, Customer>
    {
        public override void Transform(CommunicationResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Communication = new Customer.Contacts
            {
                ContactId = queryResult.Id,
                Email = queryResult.Email,
                Phone = queryResult.Telephone
            };

            if (queryResult.HouseNo != null)
                customer.Communication.PostalAddress = new Customer.Contacts.Address
                {
                    HouseNo = queryResult.HouseNo,
                    City = queryResult.City,
                    Country = queryResult.Country,
                    PostalCode = queryResult.PostalCode,
                    Region = queryResult.Region
                };
        }
    }
}