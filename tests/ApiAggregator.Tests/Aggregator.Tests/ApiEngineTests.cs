using ApiAggregator.Net;
using ApiAggregator.Net.Impl;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    internal class ApiEngineTests
    {
        private IApiEngine apiEngine;
        private Mock<ILogger<ApiEngine>> logger;
        private Mock<IHttpClientFactory> httpClientFactory;
        private Mock<IWebApi> webApi;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<ApiEngine>>();
            httpClientFactory = new Mock<IHttpClientFactory>();
            webApi = new Mock<IWebApi>();

            apiEngine = new ApiEngine(httpClientFactory.Object, logger.Object);
        }

        [Test]
        public void TestApiEngine()
        {
            apiEngine.Execute(new[] { webApi.Object });

            webApi.Verify(c => c.Run(httpClientFactory.Object, logger.Object), Times.Once());
        }
    }
}