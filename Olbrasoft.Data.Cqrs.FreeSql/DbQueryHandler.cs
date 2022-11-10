namespace Olbrasoft.Data.Cqrs.FreeSql;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : DbRequestHandler<TContext, TEntity, TQuery, TResult>
   where TEntity : class where TContext : DbContext where TQuery : BaseQuery<TResult>
{
    protected DbQueryHandler(TContext context) : base(context)
    {
    }

    protected DbQueryHandler(IConfigure<TEntity> configurator, TContext context) : base(configurator, context)
    {
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        token.ThrowIfCancellationRequested();
    }
}

public abstract class DbQueryHandler<TContext, TEntity, TQuery> : DbQueryHandler<TContext, TEntity, TQuery, bool>
  where TEntity : class where TContext : DbContext where TQuery : BaseQuery<bool>
{
    protected DbQueryHandler(TContext context) : base(context)
    {
    }
}