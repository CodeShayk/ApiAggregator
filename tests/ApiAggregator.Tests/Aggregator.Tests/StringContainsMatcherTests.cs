using ApiAggregator.Net;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    public class StringContainsMatcherTests
    {
        [TestCase("customer.orders", "customer")]
        [TestCase("customer.orders", "customer.orders")]
        [TestCase("customer.communication", "customer")]
        [TestCase("customer.orders.items", "customer.orders")]
        [TestCase("customer.orders.items", "customer")]
        public void TestMatcher(string input, string config)
        {
            var matcher = new StringContainsMatcher();

            Assert.True(matcher.IsMatch(input, With.Name(config)));
        }
    }
}