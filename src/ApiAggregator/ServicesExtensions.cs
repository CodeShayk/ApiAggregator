using ApiAggregator.Net.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregator.Net
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseApiAggregator(this IServiceCollection services)
        {
            services.AddTransient(typeof(IApiBuilder<>), typeof(ApiBuilder<>));
            services.AddTransient(typeof(IContractBuilder<>), typeof(ContractBuilder<>));
            services.AddTransient(typeof(IApiAggregator<>), typeof(ApiAggregator<>));

            services.AddTransient<IApiExecutor, ApiExecutor>();
            services.AddTransient<IApiNameMatcher, StringContainsMatcher>();
            services.AddTransient<IApiEngine, ApiEngine>();

            return services;
        }

        public static IServiceCollection AddApiAggregate<TContract>(this IServiceCollection services, IApiAggregate<TContract> apiAggregate)
            where TContract : IContract
        {
            if (apiAggregate != null)
                services.AddTransient(c => (IApiAggregate<TContract>)apiAggregate);

            return services;
        }
    }
}