using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Altairis.FutLabIS.Data {
    public class ApplicationRole : IdentityRole<int> {
        public const string RoleMember = "Member";
        public const string RoleMaster = "Master";
        public const string RoleAdministrator = "Administrator";
    }
}
