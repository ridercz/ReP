namespace Altairis.ReP.Web;

public class ImpossibleException : Exception {
    public ImpossibleException() : base("This should never happen.") { }
    public ImpossibleException(string message) : base(message) { }
    public ImpossibleException(string message, Exception innerException) : base(message, innerException) { }
}
