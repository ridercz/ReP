namespace Olbrasoft.ReP.Business;

public abstract class BaseService
{
	protected IDispatcher Dispatcher { get; }

	protected BaseService(IDispatcher dispatcher)
	{
		Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
	}

	protected static void ThrowExceptionIfStatusIsError(CommandStatus status)
	{
		if (status == CommandStatus.Error) throw new InvalidOperationException("More than one entity has been saved!");
	}
}
