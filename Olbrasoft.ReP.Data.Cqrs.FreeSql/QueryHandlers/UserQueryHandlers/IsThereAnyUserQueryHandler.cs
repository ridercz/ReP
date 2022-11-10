namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.UserQueryHandlers;
public class IsThereAnyUserQueryHandler : RepDbQueryHandler<ApplicationUser, IsThereAnyUserQuery>
{
    public IsThereAnyUserQueryHandler(RepDbContextFreeSql context) : base(context)
    {
    }

    protected override async Task<bool> GetResultToHandleAsync(IsThereAnyUserQuery query, CancellationToken token)
    => await ExistsAsync(token);
}
