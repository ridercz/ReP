namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityConfigurations;
public class ResourceAttachmentConfiguration : IEntityTypeConfiguration<ResourceAttachment>
{
    public void Configure(EfCoreTableFluent<ResourceAttachment> model)
    {
        model.Property(ra => ra.Id).Help().IsIdentity(true);
    }
}
