using IGeekFan.AspNetCore.Identity.FreeSql;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql;
public class RepDbContextFreeSql : IdentityDbContext<ApplicationUser, ApplicationRole, int> , IDbSetProvider, IDbContextProxy
{

}
