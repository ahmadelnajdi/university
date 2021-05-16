using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using University.Areas.Identity.Data;

namespace University.Data
{
    public class UniversityContext : IdentityDbContext<User>
    {
        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

       public DbSet<Paper> papers { get; set; }

        public DbSet<StudentData> studentDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
