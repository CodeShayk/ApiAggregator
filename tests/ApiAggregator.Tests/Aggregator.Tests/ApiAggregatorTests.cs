using ApiAggregator.Net;
using ApiAggregator.Net.Impl;
using ApiAggregator.Tests.ApiAggregate;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    internal class ApiAggregatorTests
    {
        private IApiAggregator<Customer> _aggregator;
        private Mock<ILogger<ApiAggregator<Customer>>> _logger;
        private Mock<IApiBuilder<Customer>> _apiBuilder;
        private Mock<IApiExecutor> _apiExecutor;
        private Mock<IContractBuilder<Customer>> _contractBuilder;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ApiAggregator<Customer>>>();
            _apiBuilder = new Mock<IApiBuilder<Customer>>();
            _apiExecutor = new Mock<IApiExecutor>();
            _contractBuilder = new Mock<IContractBuilder<Customer>>();

            _aggregator = new ApiAggregator<Customer>(_logger.Object, _apiBuilder.Object, _apiExecutor.Object, _contractBuilder.Object);
        }

        [Test]
        public void TestAggregator()
        {
            var context = new CustomerContext { CustomerId = 1 };

            _aggregator.GetData(context);

            _apiBuilder.Verify(x => x.Build(It.IsAny<IRequestContext>()), Times.Once);
            _apiExecutor.Verify(x => x.Execute(It.IsAny<IRequestContext>(), It.IsAny<IApiList>()), Times.Once);
            _contractBuilder.Verify(x => x.Build(It.IsAny<IRequestContext>(), It.IsAny<List<IApiResult>>()), Times.Once);
        }
    }
}