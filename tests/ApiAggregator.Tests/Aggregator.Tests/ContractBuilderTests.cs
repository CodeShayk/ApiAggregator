using ApiAggregator.Impl;
using ApiAggregator.Tests.ApiAggregate;
using ApiAggregator.Tests.ApiAggregate.ApiResults;
using ApiAggregator.Tests.ApiAggregate.WebApis;
using NUnit.Framework;

namespace ApiAggregator.Tests.Aggregator.Tests
{
    [TestFixture]
    internal class ContractBuilderTests
    {
        private ContractBuilder<Customer> _contractBuilder;
        private IApiAggregate<Customer> _customerAggregate;

        private static List<(Type result, int InvocationCount)> TransformerInvocations;

        [SetUp]
        public void Setup()
        {
            _customerAggregate = new MockCustomerAggregate();
            _contractBuilder = new ContractBuilder<Customer>(_customerAggregate);
            TransformerInvocations = new List<(Type result, int InvocationCount)>();
        }

        [Test]
        public void TestContractBuilderForCorrectExecutionOfConfiguredTransforms()
        {
            var apiList = new List<IApiResult>
            {
                new CustomerResult{Id = 123, Code= "ABC", Name="Ninja Labs"},
                new CommunicationResult{Id = 123, Email = "ninja@labs.com", Telephone = "0212345689"},
                new OrderResult(),
                new OrderItemResult()
            };

            var contract = _contractBuilder.Build(new CustomerContext(), apiList);

            var customerTransforms = TransformerInvocations.Where(x => x.result == typeof(CustomerResult));
            Assert.That(customerTransforms.Count() == 1);
            Assert.That(customerTransforms.ElementAt(0).InvocationCount == 1);

            var communicationTransforms = TransformerInvocations.Where(x => x.result == typeof(CommunicationResult));
            Assert.That(communicationTransforms.Count() == 1);
            Assert.That(communicationTransforms.ElementAt(0).InvocationCount == 1);

            var orderCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(OrderResult));
            Assert.That(orderCollectionTransforms.Count() == 1);
            Assert.That(orderCollectionTransforms.ElementAt(0).InvocationCount == 1);

            var orderItemsCollectionTransforms = TransformerInvocations.Where(x => x.result == typeof(OrderItemResult));
            Assert.That(orderItemsCollectionTransforms.Count() == 1);
            Assert.That(orderItemsCollectionTransforms.ElementAt(0).InvocationCount == 1);

            Assert.IsNotNull(contract);
        }

        public class MockTransform<TResult, TContract> : ResultTransformer<TResult, TContract>
        where TContract : IContract
        where TResult : IApiResult
        {
            public override void Transform(TResult apiResult, TContract contract)
            {
                TransformerInvocations.Add((apiResult.GetType(), 1));
            }
        }

        internal class MockCustomerAggregate : ApiAggregate<Customer>
        {
            public override IEnumerable<Mapping<Customer, IApiResult>> Construct()
            {
                return CreateAggregate.For<Customer>()
                    .Map<CustomerApi, MockTransform<CustomerResult, Customer>>(With.Name("customer"),
                     customer => customer.Dependents
                        .Map<CommunicationApi, MockTransform<CommunicationResult, Customer>>(With.Name("customer.communication"))
                        .Map<OrdersApi, MockTransform<OrderResult, Customer>>(With.Name("customer.orders"),
                            customerOrders => customerOrders.Dependents
                                .Map<OrderItemsApi, MockTransform<OrderItemResult, Customer>>(With.Name("customer.orders.items")))
                    ).Create();
            }
        }
    }
}