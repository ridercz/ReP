using Olbrasoft.Data.Cqrs;
using Olbrasoft.Dispatching;
using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers;

public abstract class DbQueryHandler<TEntity, TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : BaseQuery<TResult> where TEntity : class
{
    private readonly IDbSetProvider? _setProvider;
    private readonly IDataSelector? _selector;

    protected ISelect<TEntity> Select { get; set; }


    protected DbQueryHandler(IDbSetProvider setProvider)
    {
        if (setProvider is null)
            throw new ArgumentNullException(nameof(setProvider));

        _setProvider = setProvider;


        Select = _setProvider.Set<TEntity>().Select;
    }

    protected DbQueryHandler(IDataSelector selector)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        _selector = selector;

        Select = _selector.Select<TEntity>();
    }

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
    protected ISelect<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector) 
        => Select.OrderByDescending(keySelector);


    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="exp">lambda expression</param>
    /// <returns></returns>
    protected ISelect<TEntity> Where(Expression<Func<TEntity, bool>> exp) 
        => Select.Where(exp);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="column"></param>
    /// <returns></returns>
    protected ISelect<TEntity> OrderBy<TMember>(Expression<Func<TEntity, TMember>> column) 
        => Select.OrderBy(column);


}

public abstract class DbQueryHandler<TEntity, TQuery> : DbQueryHandler<TEntity, TQuery, bool> where TQuery : BaseQuery<bool> where TEntity : class
{
    protected DbQueryHandler(IDbSetProvider setOwner) : base(setOwner)
    {
    }
}