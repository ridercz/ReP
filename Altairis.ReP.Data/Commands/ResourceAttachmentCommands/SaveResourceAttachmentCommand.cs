using Altairis.ReP.Data.Entities;

namespace Altairis.ReP.Data.Commands.ResourceAttachmentCommands;
public class SaveResourceAttachmentCommand : BaseCommand<ResourceAttachment>
{
    public SaveResourceAttachmentCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int ResourceId { get; set; }

    [Required, MaxLength(100)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime DateCreated { get; set; }

    [Required, MaxLength(100)]
    public string StoragePath { get; set; } = string.Empty;
}
