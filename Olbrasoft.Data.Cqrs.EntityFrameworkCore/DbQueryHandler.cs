namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TContext : DbContext where TEntity : class where TQuery : BaseQuery<TResult>
{
    private IQueryable<TEntity>? _queryable;
    private readonly IProjector? _projector;

    protected virtual TContext Context { get; }

    protected IQueryable<TEntity> Queryable
    {
        get => _queryable is null ? Context.Set<TEntity>() : _queryable;
        set => _queryable = value;
    }

    protected DbQueryHandler(TContext context)
     => Context = context ?? throw new ArgumentNullException(nameof(context));

    public DbQueryHandler(IProjector projector, TContext context) : this(context)
     => _projector = projector ?? throw new ArgumentNullException(nameof(projector));

    public abstract Task<TResult> HandleAsync(TQuery request, CancellationToken token);

    protected IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
     => Queryable.Where(predicate);

    protected IOrderedQueryable<TEntity> GetOrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
     => Queryable.OrderBy(keySelector);

    protected IOrderedQueryable<TEntity> GetOrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
     => Queryable.OrderByDescending(keySelector);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
     => await ProjectTo<TDestination>(Queryable).ToArrayAsync(token);

    protected Task<TEntity?> GetOneOrNullAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
     => Queryable.SingleOrDefaultAsync(predicate, token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(IQueryable<TEntity> queryable, CancellationToken token)
     => await ProjectTo<TDestination>(queryable).ToArrayAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> predicate,
                                                                                     CancellationToken token)
     => await ProjectTo<TDestination>(GetWhere(predicate)).ToArrayAsync(token);

    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
     => source is null
            ? throw new ArgumentNullException(nameof(source))
            : _projector is null ? throw new NullReferenceException(nameof(_projector)) : _projector.ProjectTo<TDestination>(source);
}
