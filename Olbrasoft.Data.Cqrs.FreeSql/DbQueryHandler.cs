namespace Olbrasoft.Data.Cqrs.FreeSql;

public abstract class DbQueryHandler<TEntity, TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : BaseQuery<TResult> where TEntity : class
{
    private readonly IDbSetProvider? _setProvider;
    private readonly IDataSelector? _selector;
    private readonly IConfigure<TEntity>? _configurator;
    private ISelect<TEntity>? _select;

    protected ISelect<TEntity> Select
    {
        get => _select is not null ? _select : GetSelect();
        set => _select = value;
    }

    protected DbQueryHandler(IDbSetProvider setProvider)
    {
        if (setProvider is null)
            throw new ArgumentNullException(nameof(setProvider));

        _setProvider = setProvider;
    }

    protected DbQueryHandler(IDataSelector selector)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        _selector = selector;
    }

    private ISelect<TEntity> GetSelect()
        => _selector is not null
            ? _selector.Select<TEntity>()
            : _setProvider is not null
            ? _setProvider.Set<TEntity>().Select
            : throw new NullReferenceException($"GetSelect {nameof(_selector)} and {nameof(_setProvider)} is null!");

    public DbQueryHandler(IConfigure<TEntity> configurator, IDataSelector selector) : this(selector)
        => _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));

    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken token);

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        token.ThrowIfCancellationRequested();
    }

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <param name="column">列</param>
    /// <returns></returns>
    protected ISelect<TEntity> GetOrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        => Select.OrderByDescending(keySelector);

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="exp">lambda expression</param>
    /// <returns></returns>
    protected ISelect<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp)
        => Select.Where(exp);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="column"></param>
    /// <returns></returns>
    protected ISelect<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> column) => Select.OrderBy(column);
    
    protected Task<bool> AnyAsync(CancellationToken token = default) => Select.AnyAsync(token);

    protected Task<List<TDto>> ToListAsync<TDto>(CancellationToken token = default) => Select.ToListAsync<TDto>(token);

    public Expression<Func<TEntity, TDestination>> ProjectionConfigure<TDestination>() where TDestination : new()
    {
        if (_configurator is null) throw new NullReferenceException($"{nameof(_configurator)} is null !");

        return _configurator.Configure<TDestination>();
    }

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<TEntity> select, CancellationToken token)
        where TDestination : new()
       => await select.ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
       => await Select.ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(ISelect<TEntity> select, CancellationToken token)
       => await select.ToListAsync(token);

    protected async Task<TEntity> GetOneOrNullAsync(ISelect<TEntity> select, CancellationToken token)
       => await select.ToOneAsync(token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<TEntity> select, CancellationToken token) where TDestination : new() 
       => await select.ToOneAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<TEntity> GetOneOrNullAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
       => await GetOneOrNullAsync(Select.Where(exp), token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
        where TDestination : new()
       => await GetOneOrNullAsync<TDestination>(Select.Where(exp), token);
}

public abstract class DbQueryHandler<TEntity, TQuery> : DbQueryHandler<TEntity, TQuery, bool> where TQuery : BaseQuery<bool> where TEntity : class
{
    protected DbQueryHandler(IDbSetProvider setOwner) : base(setOwner)
    {
    }
}