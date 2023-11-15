using System.Globalization;
using Altairis.Services.DateProvider;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Templating;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class CreateModel(RepDbContext dc, IDateProvider dateProvider, ITemplatedMailerService mailer) : PageModel {
    private readonly RepDbContext dc = dc ?? throw new ArgumentNullException(nameof(dc));
    private readonly IDateProvider dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
    private readonly ITemplatedMailerService mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, DataType("Markdown")]
        public string Text { get; set; }

    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        // Create news
        var newMessage = new NewsMessage {
            Date = this.dateProvider.Now,
            Text = this.Input.Text,
            Title = this.Input.Title
        };
        this.dc.NewsMessages.Add(newMessage);
        await this.dc.SaveChangesAsync();

        // Send mailing
        var msg = new TemplatedMailMessageDto("News");
        var recipients = await this.dc.Users.Where(x => x.SendNews).Select(x => new { x.Email, x.UserName, x.Language }).ToListAsync();
        foreach (var item in recipients) {
            msg.To.Clear();
            msg.To.Add(new MailAddressDto(item.Email, item.UserName));
            var culture = new CultureInfo(item.Language);
            await this.mailer.SendMessageAsync(msg, new { title = newMessage.Title, text = newMessage.Text }, culture, culture);
        }

        return this.RedirectToPage("Index", null, "created");
    }

}
