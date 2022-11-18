using Microsoft.EntityFrameworkCore.ChangeTracking;
using Olbrasoft.Dispatching;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;
public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
   where TContext : DbContext where TEntity : class where TCommand : BaseCommand<TResult>
{

    private DbSet<TEntity>? _entities;
    protected TCommand? Command;
    private readonly IMapper? _mapper;
    

    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    protected DbCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    protected DbCommandHandler(IMapper mapper, TContext context) : this(context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public DbCommandHandler(IProjector projector, IMapper mapper, TContext context):this(projector, context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    protected DbSet<TEntity> Entities { get => _entities is null ? Context.Set<TEntity>() : _entities; private set => _entities = value; }

    protected void ThrowIfCommandStatusCannotBeSet(CommandStatus status)
    {
        if (TrySetCommandStatus(status))
            return;
        throw new InvalidOperationException($"Failed to set command status: {status}");
    }

    protected bool TrySetCommandStatus(CommandStatus status)
    {
        if (Command is null) return false;

        Command.Status = status;

        return true;
    }

    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        token.ThrowIfCancellationRequested();
    }

    protected virtual Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return Context.SaveChangesAsync(token);
    }

    protected virtual EntityState GetEntityState(object entity) => Context.Entry(entity).State;

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token = default)
    {
        var entity = await GetOneOrNullAsync(exp, token);

        if (entity is not null) return await RemoveAndSaveAsync(entity, token);
        
        TrySetCommandStatus(CommandStatus.NotFound);

        return CommandStatus.NotFound;
    }

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(TEntity detachedOrUnchangedEntity, CancellationToken token = default)
    {
        var state = GetEntityState(detachedOrUnchangedEntity);

        if (state == EntityState.Detached || state == EntityState.Unchanged)
        {
            Remove(detachedOrUnchangedEntity);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Deleted);
                return CommandStatus.Deleted;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> AddAndSaveAsync(TEntity detachedEntity, CancellationToken token = default)
    {
        if (GetEntityState(detachedEntity) == EntityState.Detached)
        {
            await AddAsync(detachedEntity, token);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Created);
                return CommandStatus.Created;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> UpdateAndSaveAsync(TEntity unchangedEntity, CancellationToken token = default)
    {
        Entities.Update(unchangedEntity);

        TrySetCommandStatus(CommandStatus.Modified);

        return await SaveAsync(unchangedEntity, token);
    }

    protected virtual async Task<CommandStatus> SaveAsync(TEntity modifiedEntity, CancellationToken token = default)
    {
        var state = GetEntityState(modifiedEntity);

        if (state == EntityState.Unchanged)
        {
            TrySetCommandStatus(CommandStatus.Unchanged);
            return CommandStatus.Unchanged;
        }

        if (state == EntityState.Modified)
        {
            TrySetCommandStatus(CommandStatus.Modified);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Success);
                return CommandStatus.Success;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<bool> SaveOneEntityAsync(CancellationToken token = default)
    {
        return await Context.SaveChangesAsync(token) == 1;
    }

    protected virtual EntityEntry<TEntity> Remove(TEntity entity)
    {
        var result = Entities.Remove(entity);
        TrySetCommandStatus(CommandStatus.Removed);
        return result;
    }

    protected virtual async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await Entities.AddAsync(entity, cancellationToken);

        TrySetCommandStatus(CommandStatus.Added);

        return result;
    }

    protected virtual EntityEntry<TEntity> Update(TEntity entity)
    {
        var result = Entities.Update(entity);
        TrySetCommandStatus(CommandStatus.Modified);
        return result;
    }
      

    

    /// <summary>
    /// Execute a mapping from the command to a new entity.
    /// The source type is inferred from the source object.
    /// </summary>
    /// <param name="command">TCommand to map from</param>
    /// <returns>Mapped entity</returns>
    protected TEntity MapCommandToNewEntity(TCommand command)
        => _mapper is null ? throw new NullReferenceException(nameof(_mapper)) : _mapper.MapSourceToNewDestination<TEntity>(command);

    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        if (_mapper is null) throw new NullReferenceException(nameof(_mapper));
        _mapper.MapSourceToExistingDestination(command, entity);
        return entity;
    }

   

}

