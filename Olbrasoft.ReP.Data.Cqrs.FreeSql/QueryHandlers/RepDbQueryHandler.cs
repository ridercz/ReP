namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;

public abstract class RepDbQueryHandler<TEntity, TQuery, TResult> : DbQueryHandler<TEntity, TQuery, TResult> 
    where TQuery : BaseQuery<TResult> where TEntity : class
{
    protected RepDbQueryHandler(IDataSelector selector) : base(selector)
    {
    }

    protected RepDbQueryHandler(IConfigure<TEntity> projectionConfigurator, IDataSelector selector) : base(projectionConfigurator, selector)
    {
    }

    public override Task<TResult> HandleAsync(TQuery query, CancellationToken token)
    {
        ThrowIfQueryIsNullOrCancellationRequested(query, token);
        return GetResultToHandleAsync(query, token);
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TQuery query, CancellationToken token);
}
