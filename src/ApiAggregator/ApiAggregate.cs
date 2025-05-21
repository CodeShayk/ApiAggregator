using System.Collections.Generic;

namespace ApiAggregator
{
    /// <summary>
    /// Implement the api aggregate with web apis and result transformers to map data to aggregated contract.
    /// </summary>
    /// <typeparam name="TContract">Aggregated Contract</typeparam>
    public abstract class ApiAggregate<TContract> : IApiAggregate<TContract> where TContract : IContract
    {
        public IEnumerable<Mapping<TContract, IApiResult>> Mappings { get; }

        public ApiAggregate()
        {
            Mappings = Construct();
        }

        /// <summary>
        /// Implement to configure mappings with Apis & result transformers.
        /// </summary>
        /// <returns>Entity Schema mappings</returns>
        public abstract IEnumerable<Mapping<TContract, IApiResult>> Construct();
    }
}