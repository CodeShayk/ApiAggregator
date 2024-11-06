using System.Data;
using ApiAggregator.Helpers;
using Microsoft.Extensions.Logging;

namespace ApiAggregator.Impl
{
    internal class ApiAggregator<TContract> : IApiAggregator<TContract>
        where TContract : IContract, new()
    {
        private readonly ILogger<IApiAggregator<TContract>> logger;
        private readonly IApiExecutor apiExecutor;
        private readonly IApiBuilder<TContract> apiBuilder;
        private readonly IContractBuilder<TContract> contractBuilder;

        public ApiAggregator(
            ILogger<ApiAggregator<TContract>> logger,
            IApiBuilder<TContract> apiBuilder,
            IApiExecutor apiExecutor,
            IContractBuilder<TContract> contractBuilder)
        {
            this.logger = logger;
            this.apiBuilder = apiBuilder;
            this.apiExecutor = apiExecutor;
            this.contractBuilder = contractBuilder;

            Constraints.NotNull(apiBuilder);
            Constraints.NotNull(apiExecutor);
            Constraints.NotNull(contractBuilder);
        }

        public TContract GetData(IRequestContext context)
        {
            // Build apis for the aggregated contract based on the included api names
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var apis = apiBuilder.Build(context);

            watch.Stop();
            logger?.LogInformation("Api builder executed in " + watch.ElapsedMilliseconds + " ms");

            // execute all apis to get results
            watch = System.Diagnostics.Stopwatch.StartNew();
            var results = apiExecutor.Execute(context, apis);
            watch.Stop();
            logger?.LogInformation("Api executor executed in " + watch.ElapsedMilliseconds + " ms");

            // Executes configured transformers to build aggregated contract
            watch = System.Diagnostics.Stopwatch.StartNew();
            var contract = contractBuilder.Build(context, results);
            watch.Stop();
            logger?.LogInformation("Contract builder executed in " + watch.ElapsedMilliseconds + " ms");

            return contract;
        }
    }
}