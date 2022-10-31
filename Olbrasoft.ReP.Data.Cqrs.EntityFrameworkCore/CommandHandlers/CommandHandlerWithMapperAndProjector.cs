namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;
public abstract class CommandHandlerWithMapperAndProjector<TEntity, TCommand, TResult> : CommandHandlerWithMapper<TEntity, TCommand, TResult>
    where TCommand : BaseCommand<TResult> where TEntity : class
{
    private readonly IProjector _projector;

    protected CommandHandlerWithMapperAndProjector(IProjector projector, IMapper mapper, RepDbContext context) : base(mapper, context)
    {
        _projector = projector ?? throw new ArgumentNullException(nameof(projector));
    }

    /// <summary>
    /// Project the input queryable.
    /// </summary>
    /// <remarks>Projections</remarks>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Queryable source</param>
    /// <returns>Queryable result, use queryable extension methods to project and execute result</returns>
    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
    {
        return _projector.ProjectTo<TDestination>(source);
    }


}
