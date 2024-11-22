using ApiAggregator.Impl;
using ApiAggregator.Tests.ApiAggregate;
using ApiAggregator.Tests.ApiAggregate.WebApis;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    internal class ApiBuilderTests
    {
        private IApiBuilder<Customer> _apiBuilder;

        private IApiAggregate<Customer> _apiAggrgate;
        private IApiNameMatcher _apiNameMatcher;

        [SetUp]
        public void Setup()
        {
            _apiAggrgate = new CustomerAggregate();
            _apiNameMatcher = new StringContainsMatcher();
            _apiBuilder = new ApiBuilder<Customer>(_apiAggrgate, _apiNameMatcher);
        }

        [Test]
        public void TestApiBuilderForCorrectParentApiList()
        {
            var context = new CustomerContext { Names = new[] { "customer" }, CustomerId = 1 };

            var result = _apiBuilder.Build(context);

            Assert.That(result, Is.Not.Null);

            Assert.That(result.ApiNestingDepth == 0);
            Assert.That(result.Apis.Count, Is.EqualTo(1));
            Assert.That(result.Apis.ElementAt(0).Children.Count, Is.EqualTo(0));

            var parentApi = result.Apis.First();
            Assert.That(parentApi.GetType() == typeof(CustomerApi));
        }

        [Test]
        public void TestApiBuilderForCorrectParentApiListWithOneChildren()
        {
            var context = new CustomerContext { Names = new[] { "customer.communication" } };

            var result = _apiBuilder.Build(context);

            Assert.That(result, Is.Not.Null);

            Assert.That(result.ApiNestingDepth == 0);
            Assert.That(result.Apis.Count, Is.EqualTo(1));
            Assert.That(result.Apis.ElementAt(0).Children.Count, Is.EqualTo(1));

            var parentApi = result.Apis.First();
            Assert.That(parentApi.GetType() == typeof(CustomerApi));

            var childQuery = parentApi.Children.First();
            Assert.That(childQuery.GetType() == typeof(CommunicationApi));
        }

        [Test]
        public void TestApiBuilderForCorrectParentApiListWithTwoChildren()
        {
            var context = new CustomerContext { Names = new[] { "customer.communication", "customer.orders" } };

            var result = _apiBuilder.Build(context);

            Assert.That(result, Is.Not.Null);

            Assert.That(result.ApiNestingDepth == 0);
            Assert.That(result.Apis.Count, Is.EqualTo(1));
            Assert.That(result.Apis.ElementAt(0).Children.Count, Is.EqualTo(2));

            var parentApi = result.Apis.First();
            Assert.That(parentApi.GetType() == typeof(CustomerApi));

            var communicationApi = parentApi.Children.FirstOrDefault(x => x.GetType() == typeof(CommunicationApi));
            var ordersApi = parentApi.Children.FirstOrDefault(x => x.GetType() == typeof(OrdersApi));

            Assert.That(communicationApi, Is.Not.Null);
            Assert.That(ordersApi, Is.Not.Null);

            Assert.That(ordersApi.Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestApiBuilderForCorrectParentApiListWithTwoChildrenAndOneChildFurtherNestedChildApi()
        {
            var context = new CustomerContext { Names = new[] { "customer.communication", "customer.orders.items" } };

            var result = _apiBuilder.Build(context);

            Assert.That(result, Is.Not.Null);

            Assert.That(result.ApiNestingDepth == 0);
            Assert.That(result.Apis.Count, Is.EqualTo(1));
            Assert.That(result.Apis.ElementAt(0).Children.Count, Is.EqualTo(2));

            var parentApi = result.Apis.First();
            Assert.That(parentApi.GetType() == typeof(CustomerApi));

            var communicationApi = parentApi.Children.FirstOrDefault(x => x.GetType() == typeof(CommunicationApi));
            var ordersApi = parentApi.Children.FirstOrDefault(x => x.GetType() == typeof(OrdersApi));

            Assert.That(communicationApi, Is.Not.Null);
            Assert.That(ordersApi, Is.Not.Null);

            // nested child query for order item in order query children as order items are included in paths
            Assert.That(ordersApi.Children.Count, Is.EqualTo(1));

            var orderItemsApi = ordersApi.Children.FirstOrDefault(x => x.GetType() == typeof(OrderItemsApi));
            Assert.That(orderItemsApi, Is.Not.Null);
        }
    }
}