using Olbrasoft.Data.Cqrs.EntityFrameworkCore;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.CommandHandlers;

public abstract class RepDbCommandHandler<TEntity, TCommand, TResult> : DbCommandHandler<RepDbContext, TEntity, TCommand, TResult>
    where TCommand : BaseCommand<TResult> where TEntity : class
{

    protected RepDbCommandHandler(RepDbContext context) : base(context)
    {
    }

    protected RepDbCommandHandler(IMapper mapper, RepDbContext context) : base(mapper, context)
    {
    }

    protected RepDbCommandHandler(IProjector projector, IMapper mapper, RepDbContext context) : base(projector, mapper, context)
    {
    }

    public override Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        ThrowIfCommandIsNullOrCancellationRequested(command, token);

        Command = command;

        return GetResultToHandleAsync(command, token);
    }

    protected abstract Task<TResult> GetResultToHandleAsync(TCommand command, CancellationToken token);

}
