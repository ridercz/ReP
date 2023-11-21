using Altairis.ValidationToolkit;

namespace Altairis.ReP.Web.Pages.Admin;

public class OpeningHoursModel(RepDbContext dc, OpeningHoursProvider hoursProvider) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1);

        [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan OpeningTime { get; set; } = TimeSpan.Zero;

        [DataType(DataType.Time), Range(typeof(TimeSpan), "00:00:00", "23:59:59"), GreaterThan(nameof(OpeningTime), AllowEqual = true)]
        public TimeSpan ClosingTime { get; set; } = TimeSpan.Zero;

    }

    // Output model

    public IEnumerable<OpeningHoursInfo> StandardOpeningHours => hoursProvider.GetStandardOpeningHours();

    public IEnumerable<OpeningHoursChange> OpeningHoursChanges { get; set; } = [];

    // Handlers

    public async Task OnGetAsync() => this.OpeningHoursChanges = await dc.OpeningHoursChanges.OrderByDescending(x => x.Date).ToListAsync();

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        var item = await dc.OpeningHoursChanges.SingleOrDefaultAsync(x => x.Date == this.Input.Date);
        if (item == null) {
            dc.OpeningHoursChanges.Add(new OpeningHoursChange {
                Date = this.Input.Date,
                OpeningTime = this.Input.OpeningTime,
                ClosingTime = this.Input.ClosingTime
            });
        } else {
            item.OpeningTime = this.Input.OpeningTime;
            item.ClosingTime = this.Input.ClosingTime;
        }

        await dc.SaveChangesAsync();
        return this.RedirectToPage(string.Empty, null, "created");
    }

    public async Task<IActionResult> OnGetDeleteAsync(int ohchId) {
        var item = await dc.OpeningHoursChanges.FindAsync(ohchId);
        if (item == null) return this.NotFound();
        dc.OpeningHoursChanges.Remove(item);
        await dc.SaveChangesAsync();
        return this.RedirectToPage(string.Empty, null, "deleted");
    }

}
