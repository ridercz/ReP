namespace Altairis.ReP.Data;

public class ApplicationRole : IdentityRole<int> {
    public const string Master = nameof(Master);
    public const string Administrator = nameof(Administrator);
}
