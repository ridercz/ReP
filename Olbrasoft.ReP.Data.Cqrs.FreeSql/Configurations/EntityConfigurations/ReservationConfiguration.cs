namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.EntityConfigurations;
public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EfCoreTableFluent<Reservation> model)
    {
        model.Property(r => r.Id).Help().IsIdentity(true);
    }
}
