namespace ApiAggregator.Net

{
    public abstract class ResultTransformer<TApiResult, TContract> : IResultTransformer, ITransformerContext, ITransformerResultType
        where TContract : IContract
        where TApiResult : IApiResult
    {
        /// <summary>
        /// Transformer instance of data context.
        /// </summary>
        protected IRequestContext Context { get; private set; }

        /// <summary>
        /// Supported ApiResult type for the transformer.
        /// </summary>
        public Type SupportedApiResult => typeof(TApiResult);

        /// <summary>
        /// Method to set data conext for the transformer
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(IRequestContext context) => Context = context;

        /// <summary>
        /// Transform method mapping api result to Aggregate Contract.
        /// </summary>
        /// <param name="apiResult">Api Result</param>
        /// <param name="contract">Aggregate Contract</param>
        public void Transform(IApiResult apiResult, IContract contract)
        {
            Transform((TApiResult)apiResult, (TContract)contract);
        }

        /// <summary>
        /// Implement this method to map data to Aggregate Contract.
        /// </summary>
        /// <param name="apiResult">Api Result</param>
        /// <param name="contract">Aggregate Contract</param>
        public abstract void Transform(TApiResult apiResult, TContract contract);
    }
}