using System.Collections.Generic;
using System.Linq;

namespace ApiAggregator
{
    internal class ApiList : IApiList
    {
        private readonly List<IWebApi> apiList;

        public ApiList()
        {
            apiList = new List<IWebApi>();
        }

        public IEnumerable<IWebApi> Apis
        { get { return apiList; } }

        public ApiList(IEnumerable<IWebApi> collection)
        {
            apiList = new List<IWebApi>(collection);
        }

        public int ApiNestingDepth { get; set; }

        public IApiList GetByType<T>() where T : class
        {
            var apis = apiList.Where(q => q as T != null);
            return new ApiList(apis);
        }

        public List<T> As<T>() => apiList.Cast<T>().ToList();

        public List<NestedApiList> GetChildrenApis()
        {
            var childrenApis = apiList
                .Select(x => new NestedApiList { ParentApiResultType = x.ResultType, Apis = x.Children })
                .Where(x => x.Apis != null && x.Apis.Any())
                .ToList();

            return childrenApis
                .Select(x =>
                {
                    var distinctList = childrenApis
                        .Where(d => d.ParentApiResultType == x.ParentApiResultType)
                        .SelectMany(q => q.Apis)
                        .Distinct(new ApiComparer())
                        .ToList();

                    return new NestedApiList { ParentApiResultType = x.ParentApiResultType, Apis = distinctList };
                })
                .ToList();
        }

        public new int Count() => apiList.Count;

        public bool IsEmpty() => !apiList.Any();

        public void AddRange(IEnumerable<IWebApi> collection)
        {
            apiList.AddRange(collection);
        }
    }
}