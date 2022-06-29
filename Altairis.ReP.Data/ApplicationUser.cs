using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ReP.Data {
    public class ApplicationUser : IdentityUser<int> {

        public bool Enabled { get; set; } = true;

        [Required, MinLength(5), MaxLength(5)]
        public string Language { get; set; } = "cs-CZ";

        public bool SendNotifications { get; set; }

        public bool SendNews { get; set; }

    }
}
