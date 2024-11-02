using ApiAggregator.Net;

namespace ApiAggregator.Tests.CustomerAggregate.ApiResults
{
    public class OrderResult : IApiResult
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}