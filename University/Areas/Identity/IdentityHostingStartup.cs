/*using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Areas.Identity.Data;
using University.Data;

[assembly: HostingStartup(typeof(University.Areas.Identity.IdentityHostingStartup))]
namespace University.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UniversityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UniversityContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UniversityContext>();
            });
        }
    }
}*/