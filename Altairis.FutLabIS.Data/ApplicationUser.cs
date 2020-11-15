using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Altairis.FutLabIS.Data {
    public class ApplicationUser : IdentityUser<int> {

        public bool Enabled { get; set; } = true;

        [Required, MinLength(2), MaxLength(2)]
        public string Language { get; set; }

    }
}
