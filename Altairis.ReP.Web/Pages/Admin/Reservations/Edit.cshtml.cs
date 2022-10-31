using Altairis.ReP.Data.Dtos.ReservationDtos;
using Altairis.Services.Mailing.Templating;
using System.Globalization;

namespace Altairis.ReP.Web.Pages.Admin.Reservations;
public class EditModel : PageModel
{
    private readonly IReservationService _service;

    private readonly ITemplatedMailerService mailer;

    public EditModel(IReservationService service, ITemplatedMailerService mailer)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        this.mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool System { get; set; }

        public string Comment { get; set; }

    }

    public int ResourceId { get; set; }

    public string ResourceName { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; }

    public string NotificationEmail { get; set; }

    public CultureInfo NotificationCulture { get; set; }

    public async Task<ReservationEditDto> Init(int reservationId, CancellationToken token)
    {
        var r = await _service.GetReservationForEditOrNullAsync(reservationId, token);
        if (r != null)
        {
            this.ResourceId = r.ResourceId;
            this.ResourceName = r.ResourceName;
            this.UserId = r.UserId;
            this.UserName = r.UserName;

            if (r.UserSendNotifications && r.UserName != this.User.Identity.Name)
            {
                this.NotificationEmail = r.UserEmail;
                this.NotificationCulture = new CultureInfo(r.UserLanguage);
            }
        }
        return r;
    }

    public async Task<IActionResult> OnGetAsync(int reservationId, CancellationToken token)
    {
        var r = await this.Init(reservationId, token);
        if (r == null) return this.NotFound();

        this.Input = new InputModel
        {
            Comment = r.Comment,
            DateBegin = r.DateBegin,
            DateEnd = r.DateEnd,
            System = r.System
        };
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int reservationId, CancellationToken token)
    {
        var r = await this.Init(reservationId, token);
        if (r == null) return this.NotFound();

        if (!this.ModelState.IsValid) return this.Page();

        var result = await _service.SaveAsync(reservationId, this.Input.DateBegin, this.Input.DateEnd, this.Input.System, this.Input.Comment, token);

        foreach (var item in result.Conflicts)
        {
            this.ModelState.AddModelError(string.Empty, string.Format(UI.My_Reservations_Err_Conflict, item.UserName, item.DateBegin));
        }

        if (!this.ModelState.IsValid) return this.Page();

        // Send notification if time changed
        if ((r.DateBegin != this.Input.DateBegin || r.DateEnd != this.Input.DateEnd) && !string.IsNullOrEmpty(this.NotificationEmail))
        {
            var msg = new TemplatedMailMessageDto("ReservationChanged", this.NotificationEmail);
            await mailer.SendMessageAsync(msg, new
            {
                resourceName = this.ResourceName,
                userName = this.User.Identity.Name,
                oldDateBegin = r.DateBegin,
                oldDateEnd = r.DateEnd,
                dateBegin = this.Input.DateBegin,
                dateEnd = this.Input.DateEnd
            }, this.NotificationCulture, this.NotificationCulture);
        }
        return this.RedirectToPage("Index", null, "saved");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int reservationId, CancellationToken token)
    {
        var r = await this.Init(reservationId, token);
        if (r == null) return this.NotFound();

        // Send notification
        if (!string.IsNullOrEmpty(this.NotificationEmail))
        {
            var msg = new TemplatedMailMessageDto("ReservationDeleted", this.NotificationEmail);
            await this.mailer.SendMessageAsync(msg, new
            {
                resourceName = this.ResourceName,
                userName = this.User.Identity.Name,
                oldDateBegin = r.DateBegin,
                oldDateEnd = r.DateEnd,
            }, this.NotificationCulture, this.NotificationCulture);
        }

        // Delete reservation
        if (await _service.DeleteReservationAsync(reservationId, token) == CommandStatus.NotFound) return this.NotFound();

        return this.RedirectToPage("Index", null, "deleted");
    }

}
