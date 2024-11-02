using ApiAggregator.Net;

namespace ApiAggregator.Tests.CustomerAggregate
{
    internal class CustomerContext : RequestContext
    {
        public int CustomerId { get; set; }
    }
}