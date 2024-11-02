namespace ApiAggregator.Net
{
    /// <summary>
    /// Implement to configure aggregate contract with api mappings.
    /// </summary>
    /// <typeparam name="TContract">Aggregate Contract</typeparam>
    public interface IApiAggregate<TContract> where TContract : IContract
    {
        /// <summary>
        /// Entity schema mappings.
        /// </summary>
        IEnumerable<Mapping<TContract, IApiResult>> Mappings { get; }
    }
}