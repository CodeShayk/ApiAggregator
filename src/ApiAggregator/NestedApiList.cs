namespace ApiAggregator
{
    public class NestedApiList
    {
        public Type ParentApiResultType { get; set; }
        public IList<IWebApi> Apis { get; set; }
    }
}