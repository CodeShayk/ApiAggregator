namespace ApiAggregator.Tests.ApiAggregate.ApiResults
{
    public class OrderResult : ApiResult
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}