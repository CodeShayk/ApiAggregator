using System.Collections.Generic;

namespace ApiAggregator
{
    internal interface IApiExecutor
    {
        IList<IApiResult> Execute(IRequestContext context, IApiList apiList);
    }
}