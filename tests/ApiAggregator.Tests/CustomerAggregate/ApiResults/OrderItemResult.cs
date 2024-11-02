using ApiAggregator.Net;

namespace ApiAggregator.Tests.CustomerAggregate.ApiResults
{
    public class OrderItemResult : IApiResult
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}