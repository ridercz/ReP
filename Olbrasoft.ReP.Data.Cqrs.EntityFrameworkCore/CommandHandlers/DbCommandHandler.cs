using Microsoft.EntityFrameworkCore.ChangeTracking;
using Olbrasoft.Dispatching;
using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;

public abstract class DbCommandHandler<TEntity, TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : BaseCommand<TResult> where TEntity : class
{
    private readonly RepDbContext _context;

    private TCommand? _command;

    protected DbSet<TEntity> Entities { get; private set; }

    protected DbCommandHandler(RepDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        Entities = _context.Set<TEntity>();
    }

    public virtual Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        ThrowIfCommandIsNullOrCancellationRequested(command, token);

        _command = command;

        return GetResultToHandleAsync(command, token);
    }

    protected void ThrowIfCommandStatusCannotBeSet(CommandStatus status)
    {
        if (TrySetCommandStatus(status))
            return;
        throw new InvalidOperationException($"Failed to set command status: {status}");
    }

    protected bool TrySetCommandStatus(CommandStatus status)
    {
        if (_command is null) return false;

        _command.Status = status;

        return true;
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TCommand command, CancellationToken token);


    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        token.ThrowIfCancellationRequested();
    }

    protected virtual Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return _context.SaveChangesAsync(token);
    }

    protected virtual EntityState GetEntityState(object entity) => _context.Entry(entity).State;

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(TEntity detachedOrUnchangedEntity, CancellationToken token = default)
    {
        var state = GetEntityState(detachedOrUnchangedEntity);

        if (state == EntityState.Detached || state == EntityState.Unchanged)
        {
            Remove(detachedOrUnchangedEntity);

            if(await SaveOneEntityAsync(token))
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

        if (state == EntityState.Modified )
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
        return (await _context.SaveChangesAsync(token) == 1);
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

    protected virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await Entities.FirstOrDefaultAsync(predicate, cancellationToken);
        if (result is null) TrySetCommandStatus(CommandStatus.NotFound);
        return result;
    }

    protected virtual async Task<TEntity?> GetOneOrNullAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await Entities.SingleOrDefaultAsync(predicate, cancellationToken);
        if (result is null) TrySetCommandStatus(CommandStatus.NotFound);
        return result;
    }

    protected virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
      => Entities.SingleAsync(predicate, cancellationToken);

    protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        => Entities.Where(predicate);

    protected async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        => await Entities.AnyAsync(predicate, token);

}
