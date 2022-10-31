namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;

public abstract class CommandHandlerWithMapper<TEntity, TCommand, TResult> : CommandHandler<TEntity, TCommand, TResult>
    where TCommand : BaseCommand<TResult> where TEntity : class
{
    protected CommandHandlerWithMapper(IMapper mapper, RepDbContext context) : base(context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private readonly IMapper _mapper;


    /// <summary>
    /// Execute a mapping from the command to a new entity.
    /// The source type is inferred from the source object.
    /// </summary>
    /// <param name="command">TCommand to map from</param>
    /// <returns>Mapped entity</returns>
    protected TEntity MapCommandToNewEntity(TCommand command)
        => _mapper.MapSourceToNewDestination<TEntity>(command);

    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        _mapper.MapSourceToExistingDestination(command, entity);
        return entity;
    }
}