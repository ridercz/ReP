namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers;
public abstract class RepDbCommandHandler<TEntity, TCommand, TResult> : DbCommandHandler< TEntity, TCommand, TResult>
        where TCommand : BaseCommand<TResult> where TEntity : class
{
    protected RepDbCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected RepDbCommandHandler(IConfigure<TEntity> configurator, IMapper mapper, IDbContextProxy proxy) : base(configurator, mapper, proxy)
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
