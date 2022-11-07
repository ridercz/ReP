namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.UserQueryHandlers;
public class IsThereAnyUserQueryHandler : BaseQueryHandler<ApplicationUser, IsThereAnyUserQuery, bool>
{
    public IsThereAnyUserQueryHandler(RepDbContext context) : base(context)
    {
    }

    protected override async Task<bool> GetResultToHandleAsync(IsThereAnyUserQuery query, CancellationToken token)
        => await EntityQueryable.AnyAsync(token);
}
