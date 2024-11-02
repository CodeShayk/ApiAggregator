namespace ApiAggregator.Net
{
    public interface IApiAggregator<TContract> where TContract : IContract
    {
        TContract GetData(IRequestContext context);
    }
}