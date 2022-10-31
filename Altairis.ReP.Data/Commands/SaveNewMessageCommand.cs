namespace Altairis.ReP.Data.Commands;
public class SaveNewMessageCommand : BaseCommand<CommandStatus>
{
    public SaveNewMessageCommand(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
