using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.FutLabIS.Data {
    public class FutLabDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
        public FutLabDbContext(DbContextOptions options) : base(options) {
        }


        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().Property(x => x.Language).IsFixedLength();
        }
    }

    public class FutLabDbContextDesignTimeFactory : IDesignTimeDbContextFactory<FutLabDbContext> {
        public FutLabDbContext CreateDbContext(string[] args) {
            var builder = new DbContextOptionsBuilder<FutLabDbContext>();
            builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=FutLabIS_design");
            return new FutLabDbContext(builder.Options);
        }
    }
}
