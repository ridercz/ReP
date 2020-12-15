using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Altairis.FutLabIS.Data {
    public class ApplicationUser : IdentityUser<int> {

        public bool Enabled { get; set; } = true;

        [Required, MinLength(2), MaxLength(2)]
        public string Language { get; set; }

        public bool SendNotifications { get; set; }

        public bool SendNews { get; set; }

    }
}
