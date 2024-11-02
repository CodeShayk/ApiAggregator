using ApiAggregator.Net;

namespace ApiAggregator.Tests.CustomerAggregate.ApiResults
{
    [CacheResult]
    public class CommunicationResult : IApiResult
    {
        public int Id { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}