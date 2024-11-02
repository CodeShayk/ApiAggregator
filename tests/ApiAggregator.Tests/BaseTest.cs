using System;
using ApiAggregator.Net;
using ApiAggregator.Tests.CustomerAggregate.AggregateContract;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ApiAggregator.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected IServiceProvider serviceProvider;

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (serviceProvider is IDisposable disposable)
                disposable.Dispose();
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.UseApiAggregator()
                    .AddApiAggregate<Customer>(new CustomerAggregate.CustomerAggregate());

            // 4. Build the service provider
            serviceProvider = services.BuildServiceProvider();
        }
    }
}