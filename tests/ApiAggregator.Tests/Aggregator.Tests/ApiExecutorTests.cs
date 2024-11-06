using ApiAggregator.Impl;
using ApiAggregator.Tests.ApiAggregate;
using ApiAggregator.Tests.ApiAggregate.WebApis;
using Moq;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    internal class ApiExecutorTests
    {
        private IApiExecutor _queryExecutor;
        private Mock<IApiEngine> _queryEngine;

        [SetUp]
        public void Setup()
        {
            _queryEngine = new Mock<IApiEngine>();
            _queryEngine.Setup(x => x.CanExecute(It.IsAny<IWebApi>())).Returns(true);

            _queryExecutor = new ApiExecutor(_queryEngine.Object);
        }

        [Test]
        public void TestApiExecutorToReturnWhenNoApis()
        {
            _queryExecutor.Execute(new CustomerContext(), new ApiList());
            _queryEngine.Verify(x => x.Execute(It.IsAny<IEnumerable<IWebApi>>()), Times.Never());
        }

        [Test]
        public void TestApiExecutorToCallEngineWhenApisExistForExecution()
        {
            _queryExecutor.Execute(new CustomerContext(), new ApiList(new[] { new CustomerApi() }) { });

            _queryEngine.Verify(x => x.Execute(It.IsAny<IEnumerable<IWebApi>>()), Times.Once());
        }

        [Test] // TODO - All sequence assertions
        public void TestApiExecutorToExecuteConfiguredApisInCorrectOrder()
        {
            var apiList = new ApiBuilder<Customer>(new CustomerAggregate(), new StringContainsMatcher())
                .Build(new CustomerContext());

            _queryExecutor.Execute(new CustomerContext(), apiList);

            _queryEngine.Verify(x => x.Execute(It.IsAny<IEnumerable<IWebApi>>()), Times.Once());
        }
    }
}