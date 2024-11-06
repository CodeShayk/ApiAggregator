using ApiAggregator.Tests.ApiAggregate.ApiResults;

namespace ApiAggregator.Tests.ApiAggregate.WebApis
{
    public class CustomerApi : WebApi<CustomerResult>
    {
        public CustomerApi() : base(Endpoints.BaseAddress)
        {
        }

        protected override Uri GetUrl(IRequestContext context, IApiResult parentApiResult)
        {
            // Executes as root or level 1 api.
            var customerContext = (CustomerContext)context;

            return new Uri(string.Format(Endpoints.BaseAddress + Endpoints.Customer, customerContext.CustomerId), UriKind.Absolute);
        }

        protected override IDictionary<string, string> GetRequestHeaders()
        {
            return new Dictionary<string, string>
            {
                { "x-meta-branch-code", "London" }
            };
        }

        protected override IEnumerable<string> GetResponseHeaders()
        {
            return new[] { "x-meta-branch-code" };
        }
    }
}