using ApiAggregator.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregator
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseApiAggregator(this IServiceCollection services)
        {
            services.AddTransient(typeof(IApiBuilder<>), typeof(ApiBuilder<>));
            services.AddTransient(typeof(IContractBuilder<>), typeof(ContractBuilder<>));
            services.AddTransient(typeof(IApiAggregator<>), typeof(ApiAggregator<>));

            services.AddTransient(typeof(IApiExecutor), typeof(ApiExecutor));
            services.AddTransient(typeof(IApiNameMatcher), typeof(StringContainsMatcher));
            services.AddTransient(typeof(IApiEngine), typeof(ApiEngine));

            return services;
        }

        public static IServiceCollection AddApiAggregate<TContract>(this IServiceCollection services, IApiAggregate<TContract> apiAggregate)
            where TContract : IContract
        {
            if (apiAggregate != null)
                services.AddTransient(c => apiAggregate);

            return services;
        }
    }
}