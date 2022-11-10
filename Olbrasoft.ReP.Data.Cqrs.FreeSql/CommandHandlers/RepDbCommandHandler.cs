namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers;
public abstract class RepDbCommandHandler<TEntity, TCommand, TResult> : DbCommandHandler<RepDbContextFreeSql, TEntity, TCommand, TResult>
        where TCommand : BaseCommand<TResult> where TEntity : class
{
    protected RepDbCommandHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected RepDbCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected RepDbCommandHandler(IConfigure<TEntity> configurator, RepDbContextFreeSql context) : base(configurator, context)
    {
    }

    protected RepDbCommandHandler(IMapper mapper, IConfigure<TEntity> configurator, RepDbContextFreeSql context) : base(mapper, configurator, context)
    {
    }

    public override Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        ThrowIfCommandIsNullOrCancellationRequested(command, token);

        UseAutoChangeCommandStatus(command);

        return GetResultToHandleAsync(command, token);
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TCommand command, CancellationToken token);
}
