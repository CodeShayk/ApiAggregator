namespace ApiAggregator
{
    internal interface IApiBuilder<T>
    {
        IApiList Build(IRequestContext context);
    }
}