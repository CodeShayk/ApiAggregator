namespace ApiAggregator.Net
{
    public interface IRequestContextValidator
    {
        public void Validate(IRequestContext context);
    }
}