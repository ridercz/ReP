using Altairis.ReP.Data.Queries.UserQueries;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.QueryHandlers.UserQueryHandlers;
public class IsThereAnyUserQueryHandler : DbQueryHandler<ApplicationUser, IsThereAnyUserQuery>
{
    public IsThereAnyUserQueryHandler(IDbSetProvider setOwner) : base(setOwner)
    {
    }

    public override async Task<bool> HandleAsync(IsThereAnyUserQuery query, CancellationToken token) => await AnyAsync(token);
}
