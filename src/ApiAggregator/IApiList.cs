using System.Collections.Generic;

namespace ApiAggregator
{
    public interface IApiList
    {
        int ApiNestingDepth { get; set; }
        IEnumerable<IWebApi> Apis { get; }

        bool IsEmpty();

        IApiList GetByType<T>() where T : class;

        List<T> As<T>();

        List<NestedApiList> GetChildrenApis();

        void AddRange(IEnumerable<IWebApi> collection);

        int Count();
    }
}