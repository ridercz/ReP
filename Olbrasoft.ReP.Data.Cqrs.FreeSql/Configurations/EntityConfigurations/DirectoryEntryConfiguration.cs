namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class DirectoryEntryConfiguration : IEntityTypeConfiguration<DirectoryEntry>
{
    public void Configure(EfCoreTableFluent<DirectoryEntry> model)
        => model.Property(de => de.Id).Help().IsIdentity(true);
}
