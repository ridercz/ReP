namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;

public abstract class RepDbQueryHandler<TEntity, TQuery, TResult> : DbQueryHandler<RepDbContextFreeSql,TEntity, TQuery, TResult> 
    where TQuery : BaseQuery<TResult> where TEntity : class
{
    protected RepDbQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected RepDbQueryHandler(IConfigure<TEntity> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    public override Task<TResult> HandleAsync(TQuery query, CancellationToken token)
    {
        ThrowIfQueryIsNullOrCancellationRequested(query, token);
        return GetResultToHandleAsync(query, token);
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TQuery query, CancellationToken token);
}


public abstract class RepDbQueryHandler<TEntity, TQuery> : DbQueryHandler<RepDbContextFreeSql, TEntity, TQuery>
    where TEntity : class where TQuery : BaseQuery<bool>
{
    protected RepDbQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    public override Task<bool> HandleAsync(TQuery query, CancellationToken token)
    {
        ThrowIfQueryIsNullOrCancellationRequested(query, token);
        return GetResultToHandleAsync(query, token);
    }

    protected abstract Task<bool> GetResultToHandleAsync(TQuery query, CancellationToken token);
}