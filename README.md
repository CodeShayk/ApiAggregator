# <img src="https://github.com/CodeShayk/ApiAggregator/blob/master/Images/ninja-icon-16.png" alt="ninja" style="width:30px;"/> ApiAggregator.Net v1.0 
[![NuGet version](https://badge.fury.io/nu/ApiAggregator.svg)](https://badge.fury.io/nu/ApiAggregator) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CodeShayk/ApiAggregator/blob/master/LICENSE.md) 
[![Master-Build](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml) 
[![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/ApiAggregator?logo=github&sort=semver)](https://github.com/CodeShayk/ApiAggregator/releases/latest)
[![Master-CodeQL](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml) 
[![.Net 8.0](https://img.shields.io/badge/.Net-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
--
## Introduction
### What is ApiAggregator?
`ApiAggregator` is a .net utility to help combine multiple api requests to return a single aggregated response. The framework allows conditionally including a subset of configured apis to return responses.

## Using ApiAggregator
### Step 1. Create Aggregated Contract
Aggregate Contract is the resultant response from all the aggregated apis. To create aggregated contract derive the class from `IContract` interface.

Example.
```
 public class Customer : IContract
 {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Contacts Communication { get; set; }
        public Order[] Orders { get; set; }
}
```
### Step 2. Create Api Aggregated
Api Aggregate is the composition of apis to hydrate the aggrgated contract. To create an Api Aggregate derive from `ApiAggregate<TContract>` base class where TContract is Agggregated Contract(ie. Implementation of IContract).

Example.
```
internal class CustomerAggregate : ApiAggregate<Customer>
{
        /// <summary>
        /// Constructs the api aggregate with web apis and result transformers to map data to aggregated contract.
        /// </summary>
        /// <returns>Mappings</returns>
        public override IEnumerable<Mapping<Customer, IApiResult>> Construct()
        {
            return CreateAggregate.For<Customer>()
                .Map<CustomerApi, CustomerTransform>(With.Name("customer"),
                 customer => customer.Dependents
                    .Map<CommunicationApi, CommunicationTransform>(With.Name("customer.communication"))
                    .Map<OrdersApi, OrdersTransform>(With.Name("customer.orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<OrderItemsApi, OrderItemsTransform>(With.Name("customer.orders.items")))
                ).Create();
        }
}
```
`Api Aggregate` comprises of apis configured in hierarchical nested graph with each api having an associated result transformers. The result from the api is fed to the associated result transformer to map data to the aggregated contract.

#### Api & Transformer Pair
Every `Api` type in the `ApiAggregate` definition should have a complementing `Transformer` type.
You need to assign a `name` to the `api/transformer` pair.

Rules:
* You could nest api/transformer pairs in a `parent/child` hierarchy. In which case the output of the parent api will serve as the input to the nested api to resolve its api paramters.
* The api/transformer mappings can be `nested` to `5` levels down.
* By convention, it is `dot` separated string for every every nested pair, that includes all parent names separated by a dot.
Example - `customer.orders.items`

>Below is the snippet from `CustomerAggregate` definition.
```
   .Map<CustomerApi, CustomerTransform>(With.Name("customer"), -- Parent mapping with name
           customer => customer.Dependents
              .Map<CommunicationApi, CommunicationTransform>(With.Name("customer.communication")) -- nested mapping with dot separated name
```
#### Web Api Class
The purpose of a api class is to execute the api call to fetch response.

As mentioned previously, You can configure an api in `Parent` or `Child` (nested) mode in a hierarchical graph.

To define a `parent` or `nested` api you need to implement from `WebApi<TResult>` base class.

* `TResult` is the result that will be returned from executing the api.  It is an implementation of `IApiResult` type.
* Implement the `GetUrl(IRequestContext context, IApiResult parentApiResult)` method to return the constructed endpoint based on given parameters of the method.
* For Parent Api, Only `IRequestContext` context parameter is passed to GetUrl() method to resolve the Url endpoint. 
* For Nested Api, api result (ie. `IApiResult` parentApiResult parameter) from the parent api is also passed in to GetUrl() method along with IRequestContext context parameter. `IApiResult` parentApiResult parameter is null for api configured in parent mode.

`Important:` The api `endpoint` needs to be resolved before executing the api with `ApiEngine`.

Examples.


> See example `CustomerApi` implemented to be configured and run in parent mode. 
 ```
public class CustomerApi : WebApi<CustomerResult>
{
    public CustomerApi() : base(Endpoints.BaseAddress)
    {
    }

    protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
    {
        // Executes as root or level 1 api. parentApiResult should be null.
        var customerContext = (CustomerContext)context;

        return string.Format(Endpoints.BaseAddress + Endpoints.Customer, customerContext.CustomerId);
    }
}
```

> See example `CommunicationApi` implemented to be configured and run as nested api to customer api below.
```
internal class CommunicationApi : WebApi<CommunicationResult>
    {
        public CommunicationApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override string GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            var customer = (CustomerResult)parentApiResult;
            return string.Format(Endpoints.BaseAddress + Endpoints.Communication, customer.Id);
        }
    }
```
##### Result Tranformer Class
The purpose of the transformer class is to map the result fetched by the linked api  to the aggrgated contract.

To define a transformer class, you need to implement `ResultTransformer<TResult, TContract>`
- where TContract is Aggregated Contract implementing `IContract`. eg. Customer. 
- where TResult is Api Result from associated Query. It is an implementation of `IApiResult` interface. 

Example.

> `CustomerTransformer` is implemented to map `CustomerResult` recevied from CustomerApi to `Customer` Aggregated Contract.

```
public class CustomerTransform : ResultTransformer<CustomerResult, Customer>
    {
        public override void Transform(CustomerResult apiResult, Customer contract)
        {
            var customer = contract ?? new Customer();
            customer.Id = apiResult.Id;
            customer.Name = apiResult.Name;
            customer.Code = apiResult.Code;
        }
    }
```
### ApiAggregator Setup
`ApiAggregator` needs to setup with required dependencies.

#### IoC Registrations
With ServiceCollection, you need to register the below dependencies.
```
    // Register core services
    services.AddTransient(typeof(IApiBuilder<>), typeof(ApiBuilder<>));
    services.AddTransient(typeof(IContractBuilder<>), typeof(ContractBuilder<>));
    services.AddTransient(typeof(IApiAggregator<>), typeof(ApiAggregator<>));
    services.AddTransient<IApiExecutor, ApiExecutor>();
     services.AddTransient<IApiEnginne, ApiEngine>();

    // Register instance of IApiNameMatcher.
    services.AddTransient(c => new StringColonSeparatedMatcher());

    // Enable logging
    services.AddLogging();

    // Enable HttpClient
     services.AddHttpClient();

    // Register api aggregate definitions. eg CustomerAggregate
    services.AddTransient<IApiAggregate<Customer>, CustomerAggregate>();
```
#### With Fluent Registration Api
You could also acheieve the above registrations using fluent registration below.
```
    // Enable logging
    services.AddLogging();

    // Enable HttpClient
     services.AddHttpClient();

    // Fluent registration.
     services.UseApiAggregator()
             .AddApiAggregate<Customer>(new CustomerAggregate());
```
#### IApiAggrgator (DI)
To use Api aggregator, Inject IApiAggrgator<TContract> where TContract is IContract, using constructor & property injection method or explicity Resolve using service provider
ie. `IServiceProvider.GetService(typeof(IApiAggrgator<Customer>))`


## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)
