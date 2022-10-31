namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;

public abstract class BaseQueryHandlerWithProjector<TEntity, TQuery, TResult> : BaseQueryHandler<TEntity, TQuery, TResult> 
    where TQuery : BaseQuery<TResult> where TEntity : class
{
    private readonly IProjector _projector;

    protected BaseQueryHandlerWithProjector(IProjector projector, RepDbContext context) : base(context)
    {
        _projector = projector;
    }

    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source) => _projector.ProjectTo<TDestination>(source);
   
}