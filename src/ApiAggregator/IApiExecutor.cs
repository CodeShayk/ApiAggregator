namespace ApiAggregator.Net
{
    internal interface IApiExecutor
    {
        IList<IApiResult> Execute(IRequestContext context, IApiList apiList);
    }
}