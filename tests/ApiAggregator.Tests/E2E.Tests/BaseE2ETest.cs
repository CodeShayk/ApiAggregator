using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate;
using ApiAggregator.Tests.ApiAggregate.WebApis;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ApiAggregator.Tests.Aggregator.E2E.Tests
{
    [TestFixture]
    public class BaseE2ETest
    {
        protected IServiceProvider serviceProvider;
        protected WireMockServer server;

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (serviceProvider is IDisposable disposable)
                disposable.Dispose();

            server.Stop();

            if (server is IDisposable sdisposable)
                sdisposable.Dispose();
        }

        public void StubApi(string endpoint, object body)
        {
            // Arrange (start WireMock.Net server)
            server
              .Given(Request.Create().WithUrl(endpoint).UsingGet())
              .RespondWith(
                Response.Create()
                  .WithStatusCode(200)
                  .WithBodyAsJson(body, true)
              );

            // Act (use a HttpClient which connects to the URL where WireMock.Net is running)
            //var response = new HttpClient().GetAsync(endpoint).Result;

            // Assert
            //Check.That(response).IsEqualTo(EXPECTED_RESULT);
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            server = WireMockServer.Start(5000);

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddHttpClient();

            services.UseApiAggregator()
                    .AddApiAggregate<Customer>(new CustomerAggregate());

            // 4. Build the service provider
            serviceProvider = services.BuildServiceProvider();
        }
    }
}