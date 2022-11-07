using Altairis.ReP.Data.Commands;
using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries.NewsMessageQueries;

namespace Olbrasoft.ReP.Business;
public class NewsMessageService : BaseService, INewsMessageService
{
    public NewsMessageService(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public async Task<IEnumerable<NewsMessage>> GetNewsMessagesAsync(CancellationToken token = default)
        => await new NewsMessagesQuery(Dispatcher).ToResultAsync(token);

    public async Task<NewsMessage?> GetFirstNewsMessageOrNullAsync(CancellationToken token = default)
        => await new NewsMessageOrNullQuery(Dispatcher).ToResultAsync(token);

    public async Task<IEnumerable<RecipientDto>> GetNewsletterRecipients(CancellationToken token = default)
        => await new NewsletterRecipientsQuery(Dispatcher).ToResultAsync(token);

    public async Task SaveAsync(DateTime date, string title, string text, CancellationToken token = default)
        => await new SaveNewMessageCommand(Dispatcher) { Date = date, Title = title, Text = text }.ToResultAsync(token);

    public async Task<NewsMessageDto?> GetNewsMessageOrNullByAsync(int newsMessageId, CancellationToken token = default)
        => await new NewsMessageOrNullByIdQuery(Dispatcher) { NewsMessageId = newsMessageId }.ToResultAsync(token);

    public async Task<CommandStatus> SaveAsync(int id, string title, string text, CancellationToken token = default)
    {
       var result = await new SaveNewMessageCommand(Dispatcher) { Id = id, Title = title, Text = text }.ToResultAsync(token);

        ThrowExceptionIfStatusIsError(result);

        return result;
    }

    public async Task<CommandStatus> DeleteAsync(int id, CancellationToken token = default) 
        => await new DeleteNewsMessageCommand(Dispatcher) { NewsMessageId = id }.ToResultAsync(token);

    public async Task<IEnumerable<NewsMessageInfoDto>> GetNewsMessageInfos(CancellationToken token = default) 
        => await new NewsMessageInfosQuery(Dispatcher).ToResultAsync(token);
}