namespace Altairis.ReP.Data.Entities;

public class ApplicationUser : IdentityUser<int>
{

    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;

    [Required, MinLength(5), MaxLength(5)]
    public string Language { get; set; } = "cs-CZ";

    public bool SendNotifications { get; set; }

    public bool SendNews { get; set; }

    public bool ShowInMemberDirectory { get; set; }

}
