using ApiAggregator.Net;
using ApiAggregator.Tests.ApiAggregate;
using ApiAggregator.Tests.ApiAggregate.WebApis;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.E2E.Tests
{
    public class ApiAggregatorTests : BaseE2ETest
    {
        protected IApiAggregator<Customer> apiAggregator;

        [SetUp]
        public void Setup()
        {
            apiAggregator = serviceProvider.GetService<IApiAggregator<Customer>>();

            StubApi(string.Format(Endpoints.BaseAddress + Endpoints.Customer, Endpoints.Ids.CustomerId), new { Id = 1000, Name = "John McKinsey", Code = "THG-UY6789" });
            StubApi(string.Format(Endpoints.BaseAddress + Endpoints.Communication, Endpoints.Ids.CustomerId), new { contactId = 4567, phone = "07675998878", email = "John.McKinsy@gmail.com", postalAddress = new { addressId = 3456, houseNo = "22", city = "London", region = "London", postalCode = "W12 6GH", country = "United Kingdom" } });
            StubApi(string.Format(Endpoints.BaseAddress + Endpoints.Orders, Endpoints.Ids.CustomerId), new[] { new { orderId = 1234, orderNo = "GHK-897GB", date = "2024-01-01T00:00:00" } });
            StubApi(string.Format(Endpoints.BaseAddress + Endpoints.OrderItems, Endpoints.Ids.CustomerId, Endpoints.Ids.OrderId), new[] { new { itemId = 2244, name = "Pen", cost = 12.0 }, new { itemId = 6677, name = "Book", cost = 15.0 } });
        }

        [Test]
        public void TestDataProviderToFetchWholeEntityWhenPathsAreNull()
        {
            var customer = apiAggregator.GetData(new CustomerContext
            {
                CustomerId = Endpoints.Ids.CustomerId
            });
        }
    }
}