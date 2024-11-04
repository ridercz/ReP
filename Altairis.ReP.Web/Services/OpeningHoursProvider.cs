using System.Globalization;


namespace Altairis.ReP.Web.Services;

public class OpeningHoursProvider(IOptions<AppSettings> options, TimeProvider timeProvider, RepDbContext dc) {

    public OpeningHoursInfo GetOpeningHours(int dayOffset) => this.GetOpeningHours(timeProvider.GetLocalNow().Date.AddDays(dayOffset));

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(int dayOffsetFrom, int dayOffsetTo) => this.GetOpeningHours(timeProvider.GetLocalNow().Date.AddDays(dayOffsetFrom), timeProvider.GetLocalNow().Date.AddDays(dayOffsetTo));

    public OpeningHoursInfo GetOpeningHours(DateTime date) {
        date = date.Date;

        var ohch = dc.OpeningHoursChanges.SingleOrDefault(x => x.Date == date);
        return ohch == null
            ? this.GetStandardOpeningHours(date)
            : new OpeningHoursInfo(date, true, ohch.OpeningTime, ohch.ClosingTime);
    }

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(DateTime dateFrom, DateTime dateTo) {
        var date = dateFrom.Date;
        var ohchs = dc.OpeningHoursChanges.Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToList();
        while (date <= dateTo.Date) {
            var ohch = ohchs.SingleOrDefault(x => x.Date == date);
            yield return ohch == null
                ? this.GetStandardOpeningHours(date)
                : new OpeningHoursInfo(date, true, ohch.OpeningTime, ohch.ClosingTime);
            date = date.AddDays(1);
        }
    }

    public IEnumerable<OpeningHoursInfo> GetStandardOpeningHours() {
        var dt = DateTime.Today;
        while (dt.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) dt = dt.AddDays(1);
        for (var i = 0; i < 7; i++) {
            yield return this.GetStandardOpeningHours(dt);
            dt = dt.AddDays(1);
        }
    }

    private OpeningHoursInfo GetStandardOpeningHours(DateTime date) {
        var value = options.Value.OpeningHours.FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);
        return value == null
            ? new OpeningHoursInfo(date.Date, default, default, default) : new OpeningHoursInfo(date.Date, false, value.OpeningTime, value.ClosingTime);
    }

}

public record OpeningHoursInfo(DateTime Date, bool IsException, TimeSpan OpeningTime, TimeSpan ClosingTime) {
    public DateTime AbsoluteOpeningTime => this.Date.Add(this.OpeningTime);

    public DateTime AbsoluteClosingTime => this.Date.Add(this.ClosingTime);

    public bool IsOpen => this.ClosingTime.Subtract(this.OpeningTime) > TimeSpan.Zero;

    public override string ToString() => this.IsOpen ? $"{this.OpeningTime:hh\\:mm} - {this.ClosingTime:hh\\:mm}" : string.Empty;

}
