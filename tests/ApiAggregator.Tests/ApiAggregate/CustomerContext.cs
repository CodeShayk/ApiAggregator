using ApiAggregator.Net;

namespace ApiAggregator.Tests.ApiAggregate
{
    internal class CustomerContext : RequestContext
    {
        public int CustomerId { get; set; }
    }
}