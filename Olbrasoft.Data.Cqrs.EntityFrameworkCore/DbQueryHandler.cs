namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : DbRequestHandler<TContext, TEntity, TQuery, TResult>
    where TContext : DbContext where TEntity : class where TQuery : BaseQuery<TResult>
{

    protected DbQueryHandler(TContext context) : base(context)
    {
    }

    protected DbQueryHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

}
