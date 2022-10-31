using Olbrasoft.Dispatching;
using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public abstract class BaseQueryHandler<TEntity, TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : BaseQuery<TResult> where TEntity : class
{
    protected IQueryable<TEntity> EntityQueryable { get; set; }

    protected BaseQueryHandler(RepDbContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        EntityQueryable = context.Set<TEntity>();
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        token.ThrowIfCancellationRequested();
    }

    public virtual Task<TResult> HandleAsync(TQuery query, CancellationToken token)
    {
        ThrowIfQueryIsNullOrCancellationRequested(query, token);
        return GetResultToHandleAsync(query, token);
    }

    protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        => EntityQueryable.Where(predicate);

    protected IOrderedQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        => EntityQueryable.OrderBy(keySelector);

    protected IOrderedQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector) 
        => EntityQueryable.OrderByDescending(keySelector);

    protected Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => EntityQueryable.SingleOrDefaultAsync(predicate, cancellationToken);

    protected abstract Task<TResult> GetResultToHandleAsync(TQuery query, CancellationToken token);

}
