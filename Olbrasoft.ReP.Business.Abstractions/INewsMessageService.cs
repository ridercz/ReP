using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Entities;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface INewsMessageService
{
    Task<NewsMessage?> GetFirstNewsMessageOrNullAsync(CancellationToken token = default);
    Task<IEnumerable<NewsMessage>> GetNewsMessagesAsync(CancellationToken token = default);
    Task<IEnumerable<NewsMessageInfoDto>> GetNewsMessageInfos(CancellationToken token = default);

    Task<IEnumerable<RecipientDto>> GetNewsletterRecipients(CancellationToken token = default);
    Task SaveAsync(DateTime date, string title, string text, CancellationToken token = default);
    Task<CommandStatus> SaveAsync(int id, string title, string text, CancellationToken token = default);
    Task<CommandStatus> DeleteAsync(int id, CancellationToken token = default);
    Task<NewsMessageDto?> GetNewsMessageOrNullByAsync(int newsMessageId, CancellationToken token = default);
}