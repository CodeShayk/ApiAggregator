using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.ResultTransformers
{
    public class CommunicationTransform : ResultTransformer<CommunicationResult, Customer>
    {
        public override void Transform(CommunicationResult apiResult, Customer contract)
        {
            var customer = contract ?? new Customer();
            customer.Communication = new Customer.Contacts
            {
                ContactId = apiResult.Id,
                Email = apiResult.Email,
                Phone = apiResult.Telephone
            };

            if (apiResult.HouseNo != null)
                customer.Communication.PostalAddress = new Customer.Contacts.Address
                {
                    HouseNo = apiResult.HouseNo,
                    City = apiResult.City,
                    Country = apiResult.Country,
                    PostalCode = apiResult.PostalCode,
                    Region = apiResult.Region
                };
        }
    }
}