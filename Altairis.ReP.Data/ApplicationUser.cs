using System.Security.Cryptography;

namespace Altairis.ReP.Data;

public class ApplicationUser : IdentityUser<int> {
    private const int ResourceAuthorizationKeyLength = 30;
    private const string ResourceAuthorizationKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;

    [Required, MinLength(5), MaxLength(5)]
    public string Language { get; set; } = "cs-CZ";

    public bool SendNotifications { get; set; }

    public bool SendNews { get; set; }

    public bool ShowInMemberDirectory { get; set; }

    [Required, MaxLength(ResourceAuthorizationKeyLength)]
    public string ResourceAuthorizationKey { get; set; } = CreateResourceAuthorizationKey();

    public virtual ICollection<Reservation> Reservations { get; set; } = [];

    public static string CreateResourceAuthorizationKey() {
        var keyChars = new char[ResourceAuthorizationKeyLength];
        for (var i = 0; i < ResourceAuthorizationKeyLength; i++) {
            keyChars[i] = ResourceAuthorizationKeyChars[RandomNumberGenerator.GetInt32(ResourceAuthorizationKeyLength)];
        }
        return new string(keyChars);
    }

}
