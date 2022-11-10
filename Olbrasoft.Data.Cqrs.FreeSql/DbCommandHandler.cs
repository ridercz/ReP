using Olbrasoft.Mapping;

namespace Olbrasoft.Data.Cqrs.FreeSql;

public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
   where TEntity : class where TContext : DbContext where TCommand : BaseCommand<TResult>
{
    private TCommand? _command;
    private DbSet<TEntity>? _entities;
    private readonly IMapper? _mapper;

    protected DbSet<TEntity> Entities { get => _entities is null ? Context.Set<TEntity>() : _entities; private set => _entities = value; }

    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    protected DbCommandHandler(IConfigure<TEntity> configurator, TContext context) : base(configurator, context)
    {
    }

    protected DbCommandHandler(IMapper mapper, TContext context) : this(context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected DbCommandHandler(IMapper mapper, IConfigure<TEntity> configurator, TContext context) : this(configurator, context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected bool TrySetCommandStatus(CommandStatus status)
    {
        if (_command is null) return false;

        _command.Status = status;

        return true;
    }

    protected void UseAutoChangeCommandStatus(TCommand command)
    {
        _command = command;
    }

    protected DbSet<TForeignEntity> GetSet<TForeignEntity>() where TForeignEntity : class
    {
        return Context.Set<TForeignEntity>();
    }


    protected async Task<bool> SaveOneEntityAsync(CancellationToken token)
    {
        return await Context.SaveChangesAsync(token) == 1;
    }

    protected TDestination MapTo<TDestination>(object source) => _mapper.MapTo<TDestination>(source);

    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        token.ThrowIfCancellationRequested();
    }

    public async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        await Entities.AddAsync(entity, token);

        TrySetCommandStatus(CommandStatus.Added);
    }

    protected virtual async Task<CommandStatus> AddAndSaveAsync(TEntity detachedEntity, CancellationToken token = default)
    {
        await AddAsync(detachedEntity, token);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Created);
            return CommandStatus.Created;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> UpdateAndSaveAsync(TEntity unchangedEntity, CancellationToken token = default)
    {
        await Entities.UpdateAsync(unchangedEntity, token);

        TrySetCommandStatus(CommandStatus.Modified);

        return await SaveAsync(unchangedEntity, token);
    }

    protected virtual async Task<CommandStatus> SaveAsync(TEntity modifiedEntity, CancellationToken token = default)
    {
        TrySetCommandStatus(CommandStatus.Modified);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Success);
            return CommandStatus.Success;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token = default)
    {
        var result = await Select.AnyAsync(exp, token);

        if (!result) TrySetCommandStatus(CommandStatus.NotFound);

        return result;
    }

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token = default)
    {
        if (await Entities.RemoveAsync(exp, token) == 1)
        {
            TrySetCommandStatus(CommandStatus.Removed);

            if (await Context.SaveChangesAsync(token) == 0)
            {
                TrySetCommandStatus(CommandStatus.Deleted);
                return CommandStatus.Deleted;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(TEntity detachedOrUnchangedEntity, CancellationToken token = default)
    {

        Entities.Remove(detachedOrUnchangedEntity);

        TrySetCommandStatus(CommandStatus.Removed);

        if (await SaveOneEntityAsync(token))
        {
            TrySetCommandStatus(CommandStatus.Deleted);
            return CommandStatus.Deleted;
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    /// <summary>
    /// Execute a mapping from the command to a new entity.
    /// The source type is inferred from the source object.
    /// </summary>
    /// <param name="command">TCommand to map from</param>
    /// <returns>Mapped entity</returns>
    protected TEntity MapCommandToNewEntity(TCommand command) => GetMapper().MapSourceToNewDestination<TEntity>(command);

    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        GetMapper().MapSourceToExistingDestination(command, entity);
        return entity;
    }

    protected IMapper GetMapper() => _mapper is null ? throw new NullReferenceException(nameof(_mapper)) : _mapper;
}
