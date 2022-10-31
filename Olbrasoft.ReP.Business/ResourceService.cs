using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business;
public class ResourceService : BaseService, IResourceService
{
    public ResourceService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public async Task<CommandStatus> DeleteAsync(int id, CancellationToken token = default)
        => await new DeleteResourceCommand(Dispatcher) { ResourceId = id }.ToResultAsync(token);

    public async Task<Resource?> GetResourceByIdOrNullAsync(int resourceId, CancellationToken token = default)
        => await new ResourceOrNullByIdQuery(Dispatcher) { ResourceId = resourceId }.ToResultAsync(token);

    public async Task<IEnumerable<ResourceInfoDto>> GetResourceInfosAsync(CancellationToken token = default)
        => await new ResourceInfosQuery(Dispatcher).ToResultAsync(token);

    public async Task<IEnumerable<Resource>> GetResourcesAsync(bool isPrivilegedUser, CancellationToken token = default)
        => await new ResourcesQuery(Dispatcher)
        {
            IsPrivilegedUser = isPrivilegedUser

        }.ToResultAsync(token);

    public async Task<IEnumerable<ResourceTagDto>> GetResourceTagsAsync(CancellationToken token = default)
        => await new ResourceTagsQuery(Dispatcher).ToResultAsync(token);

    public Task<CommandStatus> SaveAsync(string name,
                                         string? description,
                                         string? instructions,
                                         int maximumReservationTime,
                                         bool enabled,
                                         string foregroundColor,
                                         string backgroundColor,
                                         CancellationToken token = default)
        => SaveAsync(0, name, description, instructions, maximumReservationTime, enabled, foregroundColor, backgroundColor, token);

    public async Task<CommandStatus> SaveAsync(int id, string name, string? description, string? instructions, int maximumReservationTime, bool enabled, string foregroundColor, string backgroundColor, CancellationToken token = default)
        => await new SaveResourceCommand(Dispatcher)
        {
            Id = id,
            Name = name,
            Description = description,
            Instructions = instructions,
            MaximumReservationTime = maximumReservationTime,
            Enabled = enabled,
            ForegroundColor = foregroundColor,
            BackgroundColor = backgroundColor,

        }.ToResultAsync(token);
}
