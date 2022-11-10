using Olbrasoft.Data.Cqrs.EntityFrameworkCore;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public abstract class RepDbQueryHandler<TEntity, TQuery, TResult> : DbQueryHandler<RepDbContext, TEntity, TQuery, TResult>
 where TEntity : class where TQuery : BaseQuery<TResult>
{
    protected RepDbQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected RepDbQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        token.ThrowIfCancellationRequested();
    }

    public override Task<TResult> HandleAsync(TQuery query, CancellationToken token)
    {
        ThrowIfQueryIsNullOrCancellationRequested(query, token);
        return GetResultToHandleAsync(query, token);
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TQuery query, CancellationToken token);
}
