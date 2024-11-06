using System.Text.Json.Serialization;

namespace ApiAggregator.Tests.ApiAggregate.ApiResults
{
    [CacheResult]
    public class CommunicationResult : ApiResult
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