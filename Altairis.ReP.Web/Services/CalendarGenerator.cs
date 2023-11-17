using Altairis.Services.DateProvider;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace Altairis.ReP.Web;

public class CalendarGenerator {
    private readonly RepDbContext dc;
    private readonly LinkGenerator linkGenerator;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly IOptions<AppSettings> options;
    private readonly DateTime calendarStart;

    public CalendarGenerator(RepDbContext dc, IDateProvider dateProvider, LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor, IOptions<AppSettings> options) {
        this.dc = dc;
        this.linkGenerator = linkGenerator;
        this.contextAccessor = contextAccessor;
        this.options = options;
        this.calendarStart = dateProvider.Today.Subtract(this.options.Value.IcsExport.BackDays);
    }

    public async Task<IResult> GeneratePersonalCalendar(string rak) {
        // Authenticate user
        var user = await this.dc.Users.FirstOrDefaultAsync(x => x.ResourceAuthorizationKey.Equals(rak));
        if (user == null) return Results.NotFound();

        // Create calendar
        var cal = new Calendar();
        var formatString = UI.ResourceManager.GetString(nameof(UI.Cal_My_Name), new System.Globalization.CultureInfo(user.Language)) ?? throw new ImpossibleException();
        cal.AddProperty(new CalendarProperty("X-WR-CALNAME", string.Format(formatString, this.options.Value.Design.ApplicationName)));

        // Get user reservations
        var rq = from r in this.dc.Reservations
                 where r.UserId == user.Id && r.DateBegin >= this.calendarStart
                 select new {
                     r.Id,
                     r.DateBegin,
                     r.DateEnd,
                     r.Resource!.Name,
                     r.Comment
                 };

        // Add reservations
        foreach (var item in await rq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Name,
                Description = item.Comment,
                DtStart = new CalDateTime(item.DateBegin),
                DtEnd = new CalDateTime(item.DateEnd),
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Calendar",
                    fragment: new FragmentString("#reservation_" + item.Id))
            });
        }

        // Get events
        var eq = from e in this.dc.CalendarEntries
                 where e.Date > this.calendarStart
                 select new {
                     e.Id,
                     e.Date,
                     e.Title,
                     e.Comment
                 };

        // Add events
        foreach (var item in await eq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Title,
                Description = item.Comment,
                DtStart = new CalDateTime(item.Date),
                DtEnd = new CalDateTime(item.Date),
                IsAllDay = true,
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Calendar",
                    fragment: new FragmentString("#event_" + item.Id))
            });
        }


        // Serialize to ICAL
        var calSerializer = new CalendarSerializer(cal);
        var calString = calSerializer.SerializeToString();
        return Results.Text(calString, contentType: "text/calendar");
    }

    public async Task<IResult> GenerateFullCalendar(string rak) {
        // Authenticate user
        var user = await this.dc.Users.FirstOrDefaultAsync(x => x.ResourceAuthorizationKey.Equals(rak));
        if (user == null) return Results.NotFound();

        // Create calendar
        var cal = new Calendar();
        cal.AddProperty(new CalendarProperty("X-WR-CALNAME", this.options.Value.Design.ApplicationName));

        // Get user reservations
        var rq = from r in this.dc.Reservations
                 where r.DateBegin >= this.calendarStart
                 select new {
                     r.Id,
                     r.DateBegin,
                     r.DateEnd,
                     r.Resource!.Name,
                     UserName = r.User!.DisplayName,
                     UserEmail = r.User.Email,
                     r.Comment
                 };

        // Add reservations
        foreach (var item in await rq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Name,
                Description = item.Comment,
                DtStart = new CalDateTime(item.DateBegin),
                DtEnd = new CalDateTime(item.DateEnd),
                Organizer = new Organizer {
                    CommonName = item.UserName,
                    Value = new Uri("mailto:" + item.UserEmail),
                },
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Calendar",
                    fragment: new FragmentString("#reservation_" + item.Id))
            });
        }

        // Get events
        var eq = from e in this.dc.CalendarEntries
                 where e.Date > this.calendarStart
                 select new {
                     e.Id,
                     e.Date,
                     e.Title,
                     e.Comment
                 };

        // Add events
        foreach (var item in await eq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Title,
                Description = item.Comment,
                DtStart = new CalDateTime(item.Date),
                DtEnd = new CalDateTime(item.Date),
                IsAllDay = true,
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Calendar",
                    fragment: new FragmentString("#event_" + item.Id))
            });
        }


        // Serialize to ICAL
        var calSerializer = new CalendarSerializer(cal);
        var calString = calSerializer.SerializeToString();
        return Results.Text(calString, contentType: "text/calendar");
    }

    public async Task<IResult> GenerateResourceCalendar(int resourceId, string rak) {
        // Authenticate user
        var user = await this.dc.Users.FirstOrDefaultAsync(x => x.ResourceAuthorizationKey.Equals(rak));
        var resource = await this.dc.Resources.FirstOrDefaultAsync(x => x.Id == resourceId);
        if (user == null || resource == null) return Results.NotFound();

        // Create calendar
        var cal = new Calendar();
        var formatString = UI.ResourceManager.GetString(nameof(UI.Cal_Resource_Name), new System.Globalization.CultureInfo(user.Language)) ?? throw new ImpossibleException();
        cal.AddProperty(new CalendarProperty("X-WR-CALNAME", string.Format(formatString, this.options.Value.Design.ApplicationName, resource.Name)));

        // Get reservations
        var rq = from r in this.dc.Reservations
                 where r.ResourceId == resourceId && r.DateBegin >= this.calendarStart
                 select new {
                     r.Id,
                     r.DateBegin,
                     r.DateEnd,
                     Name = r.User!.DisplayName,
                     r.Comment
                 };

        // Add reservations
        foreach (var item in await rq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Name,
                Description = item.Comment,
                DtStart = new CalDateTime(item.DateBegin),
                DtEnd = new CalDateTime(item.DateEnd),
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Reservations",
                    values: new { resourceId },
                    fragment: new FragmentString("#reservation_" + item.Id))
            });
        }

        // Get events
        var eq = from e in this.dc.CalendarEntries
                 where e.Date > this.calendarStart
                 select new {
                     e.Id,
                     e.Date,
                     e.Title,
                     e.Comment
                 };

        // Add events
        foreach (var item in await eq.ToListAsync()) {
            cal.Events.Add(new() {
                Summary = item.Title,
                Description = item.Comment,
                DtStart = new CalDateTime(item.Date),
                DtEnd = new CalDateTime(item.Date),
                IsAllDay = true,
                Uid = this.linkGenerator.GetUriByPage(this.contextAccessor.HttpContext ?? throw new ImpossibleException(),
                    page: "/My/Reservations",
                    values: new { resourceId },
                    fragment: new FragmentString("#event_" + item.Id))
            });
        }

        // Serialize calendar to ICS
        var calSerializer = new CalendarSerializer(cal);
        var calString = calSerializer.SerializeToString();
        return Results.Text(calString, contentType: "text/calendar");

    }

}