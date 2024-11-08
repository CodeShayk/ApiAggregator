namespace ApiAggregator
{
    public interface IContractBuilder<out TContract> where TContract : IContract
    {
        TContract Build(IRequestContext context, IList<IApiResult> results);
    }
}