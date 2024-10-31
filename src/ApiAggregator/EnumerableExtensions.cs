using Schemio;

namespace ApiAggregator
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> GetByType<T>(this IEnumerable<IQuery> list) where T : class, IQuery
        {
            var filtered = list.Where(q => (q as T) != null);
            return filtered.Cast<T>();
        }
    }
}