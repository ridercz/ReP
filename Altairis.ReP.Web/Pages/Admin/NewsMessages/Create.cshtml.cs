using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.Services.DateProvider;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Templating;
using System.Globalization;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class CreateModel : PageModel
{
    private readonly INewsMessageService _service;
    private readonly IDateProvider dateProvider;
    private readonly ITemplatedMailerService mailer;

    public CreateModel(INewsMessageService service, IDateProvider dateProvider, ITemplatedMailerService mailer)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        this.mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
    }

    [BindProperty]
    public NewsMessageDto Input { get; set; } = new NewsMessageDto();
    
    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!this.ModelState.IsValid) return this.Page();

        // Create news
        await _service.SaveAsync(this.dateProvider.Now, this.Input.Title, this.Input.Text, token);

        // Send mailing
        var msg = new TemplatedMailMessageDto("News");

        foreach (var item in await _service.GetNewsletterRecipients(token))
        {
            msg.To.Clear();
            msg.To.Add(new MailAddressDto(item.Email, item.Name));
            var culture = new CultureInfo(item.Language);
            await this.mailer.SendMessageAsync(msg, new { title = this.Input.Title, text = this.Input.Text }, culture, culture);
        }

        return this.RedirectToPage("Index", null, "created");
    }

}
