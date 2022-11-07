namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EfCoreTableFluent<ApplicationUser> model)
    {
        model.Property(u => u.Id).Help().IsIdentity(true);
    }
}
