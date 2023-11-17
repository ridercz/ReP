using Altairis.Services.DateProvider;
using FluentStorage.Utils.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ReP.Web.Pages.My;

public class JournalModel(RepDbContext dc, IOptions<AppSettings> options, UserManager<ApplicationUser> userManager, IDateProvider dateProvider, JournalAttachmentProcessor attachmentProcessor) : PageModel {
    private readonly RepDbContext dc = dc;
    private readonly IOptions<AppSettings> options = options;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IDateProvider dateProvider = dateProvider;
    private readonly JournalAttachmentProcessor attachmentProcessor = attachmentProcessor;

    public IEnumerable<JournalEntry> Items { get; set; } = Enumerable.Empty<JournalEntry>();

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        public int? ResourceId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, DataType("Markdown")]
        public string Text { get; set; } = string.Empty;

        public IFormFileCollection? Attachments { get; set; }

    }

    // Output model

    public bool CanAddEntry { get; set; }

    public ICollection<SelectListItem> AvailableResources { get; set; } = new List<SelectListItem>() { new() { Text = UI.My_Journal_ResourceUnspecified, Value = string.Empty } };

    // Handlers

    public async Task<IActionResult> OnGetAsync() {
        if (!this.options.Value.Features.UseJournal) return this.NotFound();

        await this.Init();

        this.Items = await this.dc.JournalEntries
            .Include(x => x.User)
            .Include(x => x.Resource)
            .Include(x => x.Attachments)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync();
        this.CanAddEntry = !this.options.Value.Journal.OnlyMastersCanWrite || this.User.IsInRole(ApplicationRole.Master) || this.User.IsInRole(ApplicationRole.Master);

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!this.options.Value.Features.UseJournal) return this.NotFound();
        await this.Init();
        if (!this.ModelState.IsValid) return this.Page();

        // Create new entry
        var newEntry = new JournalEntry {
            ResourceId = this.Input.ResourceId,
            Title = this.Input.Title,
            Text = this.Input.Text,
            DateCreated = this.dateProvider.Now,
            UserId = int.Parse(this.userManager.GetUserId(this.User) ?? throw new ImpossibleException())
        };
        this.dc.JournalEntries.Add(newEntry);
        await this.dc.SaveChangesAsync();

        // Save attachments
        if (this.options.Value.Features.UseAttachments && this.Input.Attachments != null) {
            foreach (var a in this.Input.Attachments) {
                await this.attachmentProcessor.CreateAttachment(a, newEntry.Id);
            }
        }

        // Save changes
        return this.RedirectToPage(pageName: null, pageHandler: null, fragment: "created");
    }

    public async Task<IActionResult> OnGetAttachmentAsync(int attachmentId) {
        if (!this.options.Value.Features.UseJournal || !this.options.Value.Features.UseAttachments) return this.NotFound();
        try {
            var result = await this.attachmentProcessor.GetAttachment(attachmentId);
            return this.File(result.Item1, "application/octet-stream", result.Item2);
        } catch (FileNotFoundException) {
            return this.NotFound();
        }
    }

    // Helpers

    public async Task Init() {
        var q = from r in this.dc.Resources
                orderby r.Name
                select new SelectListItem {
                    Text = r.Name,
                    Value = r.Id.ToString()
                };
        if (!this.User.IsInRole(ApplicationRole.Master) && !this.User.IsInRole(ApplicationRole.Administrator)) q = q.Where(x => x.Disabled == false);
        this.AvailableResources.AddRange(await q.ToListAsync());
    }
}
