namespace ApiAggregator.Net.Impl
{
    internal class ContractBuilder<TContract> : IContractBuilder<TContract>
        where TContract : IContract, new()
    {
        private readonly IApiAggregate<TContract> apiAggregate;

        public ContractBuilder(IApiAggregate<TContract> apiAggregate)
        {
            this.apiAggregate = apiAggregate;
        }

        /// <summary>
        /// Builds aggregate contract by executing configured api result transformers.
        /// </summary>
        /// <param name="context">Entity Context</param>
        /// <param name="apiResults">List of Api results</param>
        /// <returns></returns>
        public TContract Build(IRequestContext context, IList<IApiResult> apiResults)
        {
            var contract = new TContract();

            if (apiResults == null || !apiResults.Any())
                return contract;

            var mappings = apiAggregate.Mappings.ToList();

            // resolve context of each transformer so it is available inside for transformation if required.
            mappings.ForEach(mapping => (mapping.Transformer as ITransformerContext)?.SetContext(context));

            var apiNestedDepth = mappings.Max(x => x.Order);

            // iterate through transformers to build data source by order configured.
            for (var index = 1; index <= apiNestedDepth; index++)
            {
                var transformers = mappings
                    .Where(mapping => mapping.Order == index)
                    .Select(m => m.Transformer)
                    .Distinct()
                    .ToList();

                foreach (var apiResult in apiResults)
                    transformers.Where(transformer => (transformer as ITransformerResultType)?.SupportedApiResult == apiResult.GetType()).ToList()
                        .ForEach(supportedtransformer => supportedtransformer?.Transform(apiResult, contract));
            }

            return contract;
        }
    }
}