namespace ApiAggregator.Net
{
    internal interface IApiBuilder<T>
    {
        IApiList Build(IRequestContext context);
    }
}