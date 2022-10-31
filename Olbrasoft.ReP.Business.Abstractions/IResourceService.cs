using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IResourceService
{
    Task<IEnumerable<Resource>> GetResourcesAsync(bool isPrivilegedUser, CancellationToken token = default);
    Task<IEnumerable<ResourceTagDto>> GetResourceTagsAsync(CancellationToken token = default);
    Task<Resource?> GetResourceByIdOrNullAsync(int resourceId, CancellationToken token = default);
    Task<IEnumerable<ResourceInfoDto>> GetResourceInfosAsync(CancellationToken token = default);
    Task<CommandStatus> SaveAsync(string name,
                                  string? description,
                                  string? instructions,
                                  int maximumReservationTime,
                                  bool enabled,
                                  string foregroundColor,
                                  string backgroundColor,
                                  CancellationToken token = default);

    Task<CommandStatus> SaveAsync(int id,
                                  string name,
                                  string? description,
                                  string? instructions,
                                  int maximumReservationTime,
                                  bool enabled,
                                  string foregroundColor,
                                  string backgroundColor,
                                  CancellationToken token = default);
    Task<CommandStatus> DeleteAsync(int id, CancellationToken token = default);
}
