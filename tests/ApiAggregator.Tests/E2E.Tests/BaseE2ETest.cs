using ApiAggregator.Tests.ApiAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ApiAggregator.Tests.Aggregator.E2E.Tests
{
    [TestFixture]
    public class BaseE2ETest
    {
        protected WireMockServer server;
        protected ServiceProvider serviceProvider;
        protected IApiAggregator<Customer> apiAggregator;

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (serviceProvider is IDisposable disposable)
                disposable.Dispose();

            server.Stop();

            if (server is IDisposable sdisposable)
                sdisposable.Dispose();
        }

        public void StubApi(string endpoint, object body, IDictionary<string, string> headers = null)
        {
            // Arrange (start WireMock.Net server)

            var response = Response.Create()
                  .WithStatusCode(200)
                  .WithBodyAsJson(body);

            if (headers != null)
                response.WithHeaders(headers);

            server
              .Given(Request.Create().WithUrl(endpoint).UsingGet())
              .RespondWith(response);

            // Act (use a HttpClient which connects to the URL where WireMock.Net is running)
            var result = new HttpClient().GetAsync(endpoint).Result;
            var raw = result.Content.ReadAsStringAsync().Result;

            // Assert
            //Check.That(response).IsEqualTo(EXPECTED_RESULT);
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            server = WireMockServer.Start(5000);

            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());
            services.AddHttpClient();

            services.UseApiAggregator()
                    .AddApiAggregate<Customer>(new CustomerAggregate());

            // 4. Build the service provider
            serviceProvider = services.BuildServiceProvider();

            apiAggregator = serviceProvider.GetService<IApiAggregator<Customer>>();
        }
    }
}