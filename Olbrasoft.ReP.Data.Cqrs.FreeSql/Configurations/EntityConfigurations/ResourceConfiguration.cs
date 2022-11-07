namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EfCoreTableFluent<Resource> model) 
        => model.Property(r => r.Id).Help().IsIdentity(true);
}
