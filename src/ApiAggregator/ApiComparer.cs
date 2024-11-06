namespace ApiAggregator
{
    internal class ApiComparer : IEqualityComparer<IWebApi>
    {
        #region IApi

        public bool Equals(IWebApi x, IWebApi y) => x.GetType() == y.GetType();

        public int GetHashCode(IWebApi obj) => obj.GetType().GetHashCode();

        #endregion IApi
    }
}