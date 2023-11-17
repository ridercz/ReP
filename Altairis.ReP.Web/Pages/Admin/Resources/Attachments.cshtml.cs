namespace Altairis.ReP.Web.Pages.Admin.Resources {
    public class AttachmentsModel(RepDbContext dc, ResourceAttachmentProcessor attachmentProcessor, IOptions<AppSettings> options) : PageModel {
        private readonly RepDbContext dc = dc;
        private readonly ResourceAttachmentProcessor attachmentProcessor = attachmentProcessor;
        private readonly IOptions<AppSettings> options = options;

        public string ResourceName { get; set; } = string.Empty;

        public IEnumerable<ResourceAttachment> Items { get; set; } = Enumerable.Empty<ResourceAttachment>();

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            public IFormFile? File { get; set; }

        }

        public async Task<IActionResult> OnGetAsync(int resourceId) => await this.Init(resourceId) ? this.Page() : this.NotFound();

        public async Task<IActionResult> OnPostAsync(int resourceId) {
            if (!await this.Init(resourceId)) return this.NotFound();
            if (this.Input.File != null && this.Input.File.Length > 0) await this.attachmentProcessor.CreateAttachment(this.Input.File, resourceId);
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteAttachment(int attachmentId) {
            if (!this.options.Value.Features.UseAttachments) return this.NotFound();
            await this.attachmentProcessor.DeleteAttachment(attachmentId);
            return this.RedirectToPage();
        }

        private async Task<bool> Init(int resourceId) {
            if (!this.options.Value.Features.UseAttachments) return false;
            var res = await this.dc.Resources.FindAsync(resourceId);
            if (res == null) return false;

            this.ResourceName = res.Name;
            this.Items = await this.dc.ResourceAttachments.Where(x => x.ResourceId == resourceId).OrderByDescending(x => x.DateCreated).ToListAsync();
            return true;
        }

    }

}
