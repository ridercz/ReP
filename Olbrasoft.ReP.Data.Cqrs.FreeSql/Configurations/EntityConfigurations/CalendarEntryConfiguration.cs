namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations;
public class CalendarEntryConfiguration : IEntityTypeConfiguration<CalendarEntry>
{
    public void Configure(EfCoreTableFluent<CalendarEntry> model)
    {
        model.Property(ce => ce.Id).Help().IsIdentity(true);
    }
}
