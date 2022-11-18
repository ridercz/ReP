using Olbrasoft.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{
    private IQueryable<TEntity>? _queryable;
    private readonly IProjector? _projector;

    protected virtual TContext Context { get; }

    protected IQueryable<TEntity> Queryable
    {
        get => _queryable is null ? Context.Set<TEntity>() : _queryable;
        set => _queryable = value;
    }
    protected DbRequestHandler(TContext context)
       => Context = context ?? throw new ArgumentNullException(nameof(context));

    public DbRequestHandler(IProjector projector, TContext context) : this(context)
    => _projector = projector ?? throw new ArgumentNullException(nameof(projector));

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="exp">lambda expression</param>
    /// <returns>Queryable where</returns>
    protected IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp) => Queryable.Where(exp);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by ascending</typeparam>
    /// <param name="column">Selector property/column for ascending order</param>
    /// <returns>Ascending selection</returns>
    protected IOrderedQueryable<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Queryable.OrderBy(columnSelector);

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by descending</typeparam>
    /// <param name="columnSelector">Selector property/column for descending order</param>
    /// <returns>Descending selection</returns>
    protected IOrderedQueryable<TEntity> GetOrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Queryable.OrderByDescending(columnSelector);

    #region ExistsAsync

    protected Task<bool> ExistsAsync(CancellationToken token = default) => Queryable.AnyAsync(token);

    protected async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        => await Queryable.AnyAsync(predicate, token);
    
    #endregion

    #region GetEnumerableAsync

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
      => await ProjectTo<TDestination>(Queryable).ToArrayAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(IQueryable queryable, CancellationToken token)
     where TDestination : new()
     => await ProjectTo<TDestination>(queryable).ToArrayAsync(token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(IQueryable<TEntity> queryable, CancellationToken token)
     => await queryable.ToArrayAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
     where TDestination : new()
      => await ProjectTo<TDestination>(GetWhere(exp)).ToArrayAsync(token);
      
    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
     => await GetWhere(exp).ToArrayAsync(token);

    #endregion

    #region GetOneOrNullAsync
    
    protected async Task<TEntity?> GetOneOrNullAsync(IQueryable<TEntity> queryable, CancellationToken token)
          => await queryable.SingleOrDefaultAsync(token);

    protected async Task<TDestination?> GetOneOrNullAsync<TDestination>(IQueryable<TEntity> select, CancellationToken token) where TDestination : new()
          => await ProjectTo<TDestination>(select).SingleOrDefaultAsync(token);

    protected async Task<TEntity?> GetOneOrNullAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
    => await GetOneOrNullAsync(GetWhere(exp), token);

    protected async Task<TDestination?> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
    where TDestination : new()
         => await GetOneOrNullAsync<TDestination>(GetWhere(exp), token);
    
    #endregion

    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken token);

    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
       => _projector is null ? throw new NullReferenceException(nameof(_projector)) : _projector.ProjectTo<TDestination>(source);
}
