﻿using System.Globalization;
using Altairis.Services.DateProvider;

namespace Altairis.ReP.Web.Services;

public class OpeningHoursProvider(IOptions<AppSettings> optionsAccessor, IDateProvider dateProvider, RepDbContext dc) {
    private readonly IOptions<AppSettings> optionsAccessor = optionsAccessor;
    private readonly IDateProvider dateProvider = dateProvider;
    private readonly RepDbContext dc = dc;

    public OpeningHoursInfo GetOpeningHours(int dayOffset) => this.GetOpeningHours(this.dateProvider.Today.AddDays(dayOffset));

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(int dayOffsetFrom, int dayOffsetTo) => this.GetOpeningHours(this.dateProvider.Today.AddDays(dayOffsetFrom), this.dateProvider.Today.AddDays(dayOffsetTo));

    public OpeningHoursInfo GetOpeningHours(DateTime date) {
        date = date.Date;

        var ohch = this.dc.OpeningHoursChanges.SingleOrDefault(x => x.Date == date);
        return ohch == null
            ? this.GetStandardOpeningHours(date)
            : new OpeningHoursInfo {
                Date = date,
                IsException = true,
                OpeningTime = ohch.OpeningTime,
                ClosingTime = ohch.ClosingTime
            };
    }

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(DateTime dateFrom, DateTime dateTo) {
        var date = dateFrom.Date;
        var ohchs = this.dc.OpeningHoursChanges.Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToList();
        while (date <= dateTo.Date) {
            var ohch = ohchs.SingleOrDefault(x => x.Date == date);
            yield return ohch == null
                ? this.GetStandardOpeningHours(date)
                : new OpeningHoursInfo {
                    Date = date,
                    IsException = true,
                    OpeningTime = ohch.OpeningTime,
                    ClosingTime = ohch.ClosingTime
                };
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
        var value = this.optionsAccessor.Value.OpeningHours.FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);
        return value == null
            ? new OpeningHoursInfo { Date = date.Date }
            : new OpeningHoursInfo { Date = date.Date, OpeningTime = value.OpeningTime, ClosingTime = value.ClosingTime, IsException = false };
    }

}

public class OpeningHoursInfo {

    public DateTime Date { get; set; }

    public bool IsException { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public DateTime AbsoluteOpeningTime => this.Date.Add(this.OpeningTime);

    public DateTime AbsoluteClosingTime => this.Date.Add(this.ClosingTime);

    public bool IsOpen => this.ClosingTime.Subtract(this.OpeningTime) > TimeSpan.Zero;

    public override string ToString() => this.IsOpen ? $"{this.OpeningTime:hh\\:mm} - {this.ClosingTime:hh\\:mm}" : string.Empty;

}
