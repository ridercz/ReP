using Olbrasoft.Mapping;

namespace Olbrasoft.Data.Cqrs.FreeSql;

public abstract class DbCommandHandler<TEntity, TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : BaseCommand<TResult> where TEntity : class
{
    private readonly IMapper _mapper;

    private readonly IDbContextProxy _proxy;

    private TCommand? _command;

    private readonly IConfigure<TEntity>? _configurator;

    protected DbSet<TEntity> Entities { get; private set; }
    protected ISelect<TEntity> Select { get; private set; }

    protected DbCommandHandler(IMapper mapper, IDbContextProxy proxy)
    {
        _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        Entities = _proxy.Set<TEntity>();
        Select = Getselect<TEntity>();
    }

    public DbCommandHandler(IConfigure<TEntity> configurator, IMapper mapper, IDbContextProxy proxy) : this(mapper, proxy)
    {
        if (configurator is null) throw new ArgumentNullException(nameof(configurator));

        _configurator = configurator;
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

    protected Expression<Func<TEntity, TDestination>> ProjectionConfigure<TDestination>() where TDestination : new()
    {
        if (_configurator is null) throw new NullReferenceException($"{nameof(_configurator)} is null !");

        return _configurator.Configure<TDestination>();
    }


    protected ISelect<TForeignEntity> Getselect<TForeignEntity>() where TForeignEntity : class
        => _proxy.Set<TForeignEntity>().Select;

    protected Task RemoveAsync<TForeignEntity>(Expression<Func<TForeignEntity, bool>> predicate) where TForeignEntity : class
        => _proxy.Set<TForeignEntity>().RemoveAsync(predicate);

    protected DbSet<TForeignEntity> Set<TForeignEntity>() where TForeignEntity : class
        => _proxy.Set<TForeignEntity>();

    protected async Task<bool> SaveOneEntityAsync(CancellationToken token)
    {
        return await _proxy.SaveChangesAsync(token) == 1;
    }

    protected TDestination MapTo<TDestination>(object source) => _mapper.MapTo<TDestination>(source);

    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        token.ThrowIfCancellationRequested();
    }

    public abstract Task<TResult> HandleAsync(TCommand request, CancellationToken token);

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

            if (await _proxy.SaveChangesAsync(token) == 0)
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

    protected async Task<TEntity> GetOneOrNullAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToOneAsync(token);

    protected async Task<TEntity> GetOneOrNullAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
        => await GetOneOrNullAsync(Entities.Select.Where(exp), token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> exp, CancellationToken token)
        where TDestination : new()
        => await Entities.Select.Where(exp).ToListAsync(ProjectionConfigure<TDestination>(), token);

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

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="exp">lambda expression</param>
    /// <returns></returns>
    protected ISelect<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp)
        => Select.Where(exp);
}
