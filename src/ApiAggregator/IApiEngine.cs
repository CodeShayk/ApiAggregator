using System.Collections.Generic;

namespace ApiAggregator
{
    public interface IApiEngine
    {
        /// <summary>
        /// Detrmines whether an instance of api can be executed with this engine.
        /// </summary>
        /// <param name="api">instance of IWebApi.</param>
        /// <returns>Boolean; True when supported.</returns>
        bool CanExecute(IWebApi api);

        /// <summary>
        /// Executes a list of apis returning a list of aggregated results.
        /// </summary>
        /// <param name="apis">List of IWebApi instances.</param>
        /// <returns>List of api results. Instances of IApiResult.</returns>
        IEnumerable<IApiResult> Execute(IEnumerable<IWebApi> apis);
    }
}