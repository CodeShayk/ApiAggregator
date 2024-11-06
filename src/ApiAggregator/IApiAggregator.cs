namespace ApiAggregator
{
    public interface IApiAggregator<TContract> where TContract : IContract
    {
        TContract GetData(IRequestContext context);
    }
}