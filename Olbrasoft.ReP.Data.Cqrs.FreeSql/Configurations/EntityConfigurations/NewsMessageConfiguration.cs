namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class NewsMessageConfiguration : IEntityTypeConfiguration<NewsMessage>
{
    public void Configure(EfCoreTableFluent<NewsMessage> model) 
        => model.Property(nm => nm.Id).Help().IsIdentity(true);
}
