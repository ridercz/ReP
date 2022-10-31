using Altairis.ReP.Data.Entities;
using Microsoft.Extensions.Options;

namespace Altairis.ReP.Web.Pages.Admin.Resources
{
    public class AttachmentsModel : PageModel
    {
        private readonly IResourceAttachmentService _attachmentService;
        private readonly IResourceService _resourceService;
        private readonly AttachmentProcessor attachmentProcessor;
        private readonly IOptions<AppSettings> options;

        public AttachmentsModel(IResourceAttachmentService attachmetService, IResourceService resourceService, AttachmentProcessor attachmentProcessor, IOptions<AppSettings> options)
        {
            _attachmentService = attachmetService ?? throw new ArgumentNullException(nameof(attachmetService));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.attachmentProcessor = attachmentProcessor;
            this.options = options;
        }

        public string ResourceName { get; set; }

        public IEnumerable<ResourceAttachment> Items { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public IFormFile File { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int resourceId, CancellationToken token)
        {
            if (!await this.Init(resourceId, token)) return this.NotFound();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int resourceId, CancellationToken token)
        {
            if (!await this.Init(resourceId, token)) return this.NotFound();
            if ((this.Input?.File?.Length ?? 0) > 0) await attachmentProcessor.CreateAttachment(this.Input.File, resourceId);
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteAttachment(int attachmentId)
        {
            if (!this.options.Value.Features.UseAttachments) return this.NotFound();
            await this.attachmentProcessor.DeleteAttachment(attachmentId);
            return this.RedirectToPage();
        }

        private async Task<bool> Init(int resourceId, CancellationToken token)
        {
            if (!this.options.Value.Features.UseAttachments) return false;
            var res = await _resourceService.GetResourceByIdOrNullAsync(resourceId, token);
            if (res == null) return false;

            this.ResourceName = res.Name;
            this.Items = await _attachmentService.GetResourceAttachmentsByAsync(resourceId, token);
            return true;
        }
    }
}
