using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ApiAggregator.Tests
{
    public class E2ETests : BaseTest
    {
        protected IApiAggregator<Customer> apiAggregator;

        [SetUp]
        public void Setup()
        {
            apiAggregator = serviceProvider.GetService<IApiAggregator<Customer>>();
        }

        [Test, Ignore("Todo")]
        public void TestDataProviderToFetchWholeEntityWhenPathsAreNull()
        {
            var customer = apiAggregator.GetData(new CustomerContext
            {
                CustomerId = 1
            });
        }
    }
}