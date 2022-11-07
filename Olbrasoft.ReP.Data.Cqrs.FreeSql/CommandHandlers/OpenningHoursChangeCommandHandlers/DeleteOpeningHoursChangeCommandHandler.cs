namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.OpenningHoursChangeCommandHandlers;
public class DeleteOpeningHoursChangeCommandHandler : RepDbCommandHandler<OpeningHoursChange, DeleteOpeningHoursChangeCommand, CommandStatus>
{
    public DeleteOpeningHoursChangeCommandHandler(IMapper mapper, IDbContextProxy proxy) : base(mapper, proxy)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await GetOneOrNullAsync(ohch => ohch.Id == command.OpeningHoursChangeId, token);

        return openingHoursChange is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(openingHoursChange, token);
    }
}
