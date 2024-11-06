namespace ApiAggregator
{
    #region Helpers

    public class CreateAggregate
    {
        public static IMappings<T, IApiResult> For<T>() where T : IContract
            => new Mappings<T, IApiResult> { Order = 1 };
    }

    public class With
    {
        public static IApiNames Name(params string[] names)
            => new ApiNames { Names = names };
    }

    public class ApiNames : IApiNames
    {
        public string[] Names { get; set; }
    }

    #endregion Helpers

    public class Mappings<TContract, TApiResult> :
        List<Mapping<TContract, TApiResult>>,
        IMappings<TContract, TApiResult>,
        IMapOrComplete<TContract, TApiResult>
        where TContract : IContract
        where TApiResult : IApiResult
    {
        public int Order { get; set; }

        /// <summary>
        /// Map api and transformer for given api name
        /// </summary>
        /// <typeparam name="TWebApi">api type</typeparam>
        /// <typeparam name="TTransformer">transformer type</typeparam>
        /// <param name="names">given api names</param>
        /// <returns></returns>
        public IMapOrComplete<TContract, TApiResult> Map<TWebApi, TTransformer>(IApiNames names)
            where TWebApi : IWebApi, new()
            where TTransformer : IResultTransformer, new() =>
            Map<TWebApi, TTransformer>(names, null);

        /// <summary>
        /// Map api and transformer for given api name and with dependent api/transform mappings
        /// </summary>
        /// <typeparam name="TWebApi">api type</typeparam>
        /// <typeparam name="TTransformer">transformer type</typeparam>
        /// <param name="names">given api names</param>
        /// <param name="dependents">dependent mappings delegate</param>
        /// <returns></returns>
        public IMapOrComplete<TContract, TApiResult> Map<TWebApi, TTransformer>(IApiNames names, Func<IWithDependents<TContract, TApiResult>, IMap<TContract, TApiResult>> dependents)
            where TWebApi : IWebApi, new()
            where TTransformer : IResultTransformer, new()
        {
            var mapping = new Mapping<TContract, TApiResult>
            {
                Api = new TWebApi(),
                Transformer = new TTransformer(),
                Order = Order,
                SchemaPaths = names,
            };

            if (dependents != null)
                foreach (var dep in ((IMappings<TContract, TApiResult>)dependents(mapping)).GetMappings)
                {
                    dep.DependentOn ??= mapping.Api;
                    Add(dep);
                }

            Add(mapping);

            return this;
        }

        public Mappings<TContract, TApiResult> GetMappings => this;

        public IEnumerable<Mapping<TContract, TApiResult>> Create() => this;
    }

    public class Mapping<TContract, TApiResult> :
        IWithDependents<TContract, TApiResult>
        where TContract : IContract
        where TApiResult : IApiResult
    {
        public int Order { get; set; }
        public IApiNames SchemaPaths { get; set; }
        public IWebApi Api { get; set; }
        public IResultTransformer Transformer { get; set; }
        public IWebApi DependentOn { get; set; }

        public IMappings<TContract, TApiResult> Dependents => new Mappings<TContract, TApiResult> { Order = Order + 1 };
    }

    #region Fluent Interfaces

    public interface IApiNames
    {
        string[] Names { get; set; }
    }

    public interface IMap<TContract, TApiResult>
        where TContract : IContract
        where TApiResult : IApiResult
    {
        IMapOrComplete<TContract, TApiResult> Map<TWebApi, TTransformer>(IApiNames names)
            where TWebApi : IWebApi, new()
            where TTransformer : IResultTransformer, new();

        IMapOrComplete<TContract, TApiResult> Map<TWebApi, TTransformer>(IApiNames names, Func<IWithDependents<TContract, TApiResult>, IMap<TContract, TApiResult>> dependents)
            where TWebApi : IWebApi, new()
            where TTransformer : IResultTransformer, new();
    }

    public interface IMappings<TContract, TApiResult> : IMap<TContract, TApiResult>
        where TContract : IContract
        where TApiResult : IApiResult
    {
        int Order { get; set; }
        Mappings<TContract, TApiResult> GetMappings { get; }
    }

    public interface IMapOrComplete<TContract, TApiResult> : IMap<TContract, TApiResult>
        where TContract : IContract
        where TApiResult : IApiResult
    {
        IEnumerable<Mapping<TContract, TApiResult>> Create();
    }

    public interface IWithDependents<TContract, TApiResult>

        where TContract : IContract
        where TApiResult : IApiResult
    {
        IMappings<TContract, TApiResult> Dependents { get; }
    }

    #endregion Fluent Interfaces
}