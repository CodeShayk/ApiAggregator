namespace ApiAggregator
{
    internal interface IApiExecutor
    {
        IList<IApiResult> Execute(IRequestContext context, IApiList apiList);
    }
}