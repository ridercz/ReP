using IGeekFan.AspNetCore.Identity.FreeSql;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql;
public class RepDbContextFreeSql : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IDbSetProvider, IDbContextProxy
{
    protected override void OnModelCreating(ICodeFirst builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(RepDbContextFreeSql).Assembly);
    }
}
