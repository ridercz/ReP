using System.Globalization;
using Altairis.Services.Mailing.Templating;

namespace Altairis.ReP.Web.Pages.Admin.Reservations;

public class EditModel(RepDbContext dc, ITemplatedMailerService mailer) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

        public string? Comment { get; set; }

    }

    // Output model

    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string NotificationEmail { get; set; } = string.Empty;

    public CultureInfo NotificationCulture { get; set; } = CultureInfo.InvariantCulture;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int reservationId) {
        var r = await this.Init(reservationId);
        if (r == null) return this.NotFound();

        this.Input = new InputModel {
            Comment = r.Comment,
            DateBegin = r.DateBegin,
            DateEnd = r.DateEnd,
            System = r.System
        };

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int reservationId) {
        var r = await this.Init(reservationId);
        if (r == null) return this.NotFound();
        if (!this.ModelState.IsValid) return this.Page();

        // Check reservation for conflicts
        var q = from cr in dc.Reservations
                where cr.ResourceId == r.ResourceId && cr.DateBegin < this.Input.DateEnd && cr.DateEnd > this.Input.DateBegin && cr.Id != r.Id
                select new { cr.DateBegin, cr.User!.UserName };
        foreach (var item in await q.ToListAsync()) {
            this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Conflict, item.UserName, item.DateBegin));
        }
        if (!this.ModelState.IsValid) return this.Page();

        // Send notification if time changed
        if ((r.DateBegin != this.Input.DateBegin || r.DateEnd != this.Input.DateEnd) && !string.IsNullOrEmpty(this.NotificationEmail)) {
            var msg = new TemplatedMailMessageDto("ReservationChanged", this.NotificationEmail);
            await mailer.SendMessageAsync(msg, new {
                resourceName = this.ResourceName,
                userName = this.User.Identity!.Name,
                oldDateBegin = r.DateBegin,
                oldDateEnd = r.DateEnd,
                dateBegin = this.Input.DateBegin,
                dateEnd = this.Input.DateEnd
            }, this.NotificationCulture, this.NotificationCulture);
        }

        // Update reservation
        r.Comment = this.Input.Comment;
        r.DateBegin = this.Input.DateBegin;
        r.DateEnd = this.Input.DateEnd;
        r.System = this.Input.System;

        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int reservationId) {
        var r = await this.Init(reservationId);
        if (r == null) return this.NotFound();

        // Send notification
        if (!string.IsNullOrEmpty(this.NotificationEmail)) {
            var msg = new TemplatedMailMessageDto("ReservationDeleted", this.NotificationEmail);
            await mailer.SendMessageAsync(msg, new {
                resourceName = this.ResourceName,
                userName = this.User.Identity!.Name,
                oldDateBegin = r.DateBegin,
                oldDateEnd = r.DateEnd,
            }, this.NotificationCulture, this.NotificationCulture);
        }

        // Delete reservation
        dc.Reservations.Remove(r);
        await dc.SaveChangesAsync();
        return this.RedirectToPage("Index", null, "deleted");
    }

    // Helpers

    public async Task<Reservation?> Init(int reservationId) {
        var r = await dc.Reservations.Include(x => x.Resource).Include(x => x.User).SingleOrDefaultAsync(x => x.Id == reservationId);
        if (r != null) {
            this.ResourceId = r.ResourceId;
            this.ResourceName = r.Resource!.Name;
            this.UserId = r.UserId;
            this.UserName = r!.User!.UserName ?? throw new ImpossibleException();
            if (r.User.SendNotifications && r.User.UserName != this.User.Identity!.Name) {
                this.NotificationEmail = r!.User!.Email ?? throw new ImpossibleException();
                this.NotificationCulture = new CultureInfo(r.User.Language);
            }
        }
        return r;
    }

}
