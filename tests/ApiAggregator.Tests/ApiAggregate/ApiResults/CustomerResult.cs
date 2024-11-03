using ApiAggregator.Net;

namespace ApiAggregator.Tests.ApiAggregate.ApiResults
{
    public class CustomerResult : IApiResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}