using Altairis.ReP.Data.Commands.OpenningHoursChangeCommands;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.CommandHandlers.OpenningHoursChangeCommandHandlers;
public class DeleteOpeningHoursChangeCommandHandler : RepDbCommandHandler<OpeningHoursChange, DeleteOpeningHoursChangeCommand, CommandStatus>
{
    public DeleteOpeningHoursChangeCommandHandler(IMapper mapper, RepDbContextFreeSql context) : base(mapper, context)
    {
    }

    protected override async Task<CommandStatus> GetResultToHandleAsync(DeleteOpeningHoursChangeCommand command, CancellationToken token)
    {
        var openingHoursChange = await GetOneOrNullAsync(ohch => ohch.Id == command.OpeningHoursChangeId, token);

        return openingHoursChange is null ? CommandStatus.NotFound : await RemoveAndSaveAsync(openingHoursChange, token);
    }
}
