using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Olbrasoft.Data.Cqrs;

namespace Altairis.ReP.Web.Pages.Admin.NewsMessages;
public class EditModel : PageModel
{
    private readonly INewsMessageService _service;

    public EditModel(INewsMessageService service) 
        => _service = service ?? throw new ArgumentNullException(nameof(service));

    [BindProperty]
    public NewsMessageDto Input { get; set; } = new NewsMessageDto();

    public async Task<IActionResult> OnGetAsync(int newsMessageId)
    {
        var newsMessageDto = await _service.GetNewsMessageOrNullByAsync(newsMessageId);
        if (newsMessageDto == null) return this.NotFound();

        this.Input = newsMessageDto;

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int newsMessageId, CancellationToken token) 
        =>  ModelState.IsValid
            ? await _service.SaveAsync(newsMessageId, this.Input.Title, this.Input.Text, token) == CommandStatus.NotFound
            ? this.NotFound()
            : this.RedirectToPage("Index", null, "saved")
            : this.Page();

    public async Task<IActionResult> OnPostDeleteAsync(int newsMessageId, CancellationToken token) 
        => await _service.DeleteAsync(newsMessageId, token) == CommandStatus.NotFound
            ? this.NotFound()
            : this.RedirectToPage("Index", null, "deleted");
}
