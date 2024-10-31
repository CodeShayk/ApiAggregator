using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schemio;
using Schemio.Impl;

namespace ApiAggregator
{
    public static class ServicesExtensions
    {
        public static IServiceCollection UseApiAggregator(this IServiceCollection services, Func<IEntity, IEntitySchema<IEntity>> schemas)
        {
            services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddTransient(typeof(ITransformExecutor<>), typeof(TransformExecutor<>));
            services.AddTransient(typeof(IDataProvider<>), typeof(DataProvider<>));
            //services.AddTransient(typeof(IEntitySchema<>), typeof(BaseEntitySchema<>));

            services.AddTransient<IQueryExecutor, QueryExecutor>();
            services.AddTransient<ISchemaPathMatcher, ColonSeparatedMatcher>();
            services.AddTransient<IQueryEngine, QueryEngine>();

            //services.AddTransient((c) => schema);

            return services;
        }

        public static IServiceCollection AddEntitySchema<TEntity>(this IServiceCollection services, IEntitySchema<IEntity> schema)
            where TEntity : IEntity
        {
            if (schema != null)
                services.AddTransient(c => (IEntitySchema<TEntity>)schema);

            return services;
        }
    }
}