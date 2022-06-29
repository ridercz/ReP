using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Data;
public class ApplicationRole : IdentityRole<int> {
    public const string Master = "Master";
    public const string Administrator = "Administrator";
}
