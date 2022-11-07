namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class OpenningHoursChangeConfiguration : IEntityTypeConfiguration<OpeningHoursChange>
{
    public void Configure(EfCoreTableFluent<OpeningHoursChange> model) 
        => model.Property(o => o.Id).Help().IsIdentity(true);
}
