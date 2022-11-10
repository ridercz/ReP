namespace Olbrasoft.Data.Cqrs.FreeSql;
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{
    private readonly IConfigure<TEntity>? _configurator;

    private ISelect<TEntity>? _select;

    protected virtual TContext Context { get; }

    protected ISelect<TEntity> Select
    {
        get => _select is not null ? _select : GetSelect();

        set => _select = value;
    }

    protected DbRequestHandler(TContext context)
        => Context = context ?? throw new ArgumentNullException(nameof(context));

    protected DbRequestHandler(IConfigure<TEntity> configurator, TContext context) : this(context)
       => _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));


    protected Expression<Func<TEntity, TDestination>> ProjectionConfigure<TDestination>() where TDestination : new()
        => _configurator is null
            ? throw new NullReferenceException($"{nameof(_configurator)} is null !")
            : _configurator.Configure<TDestination>();

    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken token);

    /// <summary>
    /// Represents the select keyword from Structured Query Language.
    /// </summary>
    /// <returns>Select</returns>
    private ISelect<TEntity> GetSelect() => Context.Set<TEntity>().Select;

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="exp">lambda expression</param>
    /// <returns>Select where</returns>
    protected ISelect<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp) => Select.Where(exp);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by ascending</typeparam>
    /// <param name="column">Selector property/column for ascending order</param>
    /// <returns>Ascending selection</returns>
    protected ISelect<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Select.OrderBy(columnSelector);

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by descending</typeparam>
    /// <param name="columnSelector">Selector property/column for descending order</param>
    /// <returns>Descending selection</returns>
    protected ISelect<TEntity> GetOrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Select.OrderByDescending(columnSelector);

    protected Task<bool> ExistsAsync(CancellationToken token = default) => Select.AnyAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
      => await Select.ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<TEntity> select, CancellationToken token)
      where TDestination : new()
      => await select.ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToListAsync(token);

    protected async Task<TEntity> GetOneOrNullAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToOneAsync(token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<TEntity> select, CancellationToken token) where TDestination : new()
      => await select.ToOneAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
     where TDestination : new()
      => await GetWhere(exp).ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
      => await GetWhere(exp).ToListAsync(token);

    protected async Task<TEntity> GetOneOrNullAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
      => await GetOneOrNullAsync(GetWhere(exp), token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
        where TDestination : new()
      => await GetOneOrNullAsync<TDestination>(GetWhere(exp), token);
}
