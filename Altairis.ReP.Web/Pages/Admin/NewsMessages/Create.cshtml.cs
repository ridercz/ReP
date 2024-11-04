using System.Globalization;

using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Templating;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class CreateModel(RepDbContext dc, TimeProvider timeProvider, ITemplatedMailerService mailer) : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, DataType("Markdown")]
        public string Text { get; set; } = string.Empty;

    }

    // Handlers

    public async Task<IActionResult> OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        // Create news
        var newMessage = new NewsMessage {
            Date = timeProvider.GetLocalNow().DateTime,
            Text = this.Input.Text,
            Title = this.Input.Title
        };
        dc.NewsMessages.Add(newMessage);
        await dc.SaveChangesAsync();

        // Send mailing
        var msg = new TemplatedMailMessageDto("News");
        var recipients = await dc.Users.Where(x => x.SendNews).Select(x => new { x.Email, x.UserName, x.Language }).ToListAsync();
        foreach (var item in recipients) {
            msg.To.Clear();
            msg.To.Add(new MailAddressDto(item.Email, item.UserName));
            var culture = new CultureInfo(item.Language);
            await mailer.SendMessageAsync(msg, new { title = newMessage.Title, text = newMessage.Text }, culture, culture);
        }

        return this.RedirectToPage("Index", null, "created");
    }

}
