using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PickMeApp.Core.Models;
using System;
using System.Linq;

namespace PickMeApp.Application
{
    public static class SeedData
    {
        public static IConfiguration Configuration { get; set; }
        ///private static IHostingEnvironment Environment { get; set; }

        public static void Initialize(IServiceProvider services)
        {
            // Configuration provider
            Configuration = services.GetRequiredService<IConfiguration>();

            //Environment = services.GetRequiredService<IHostingEnvironment>();

            // Perform the initial DB migration if the DB doesn't exist
            // Eliminates need to call Update-Database during project setup
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            //Seed roles
            if (!context.Roles.Any())
            {
                context.Roles.Add(new ApplicationRole()
                {
                    Name = "Admin",
                    Description = "Admin User",
                    NormalizedName = "ADMIN"
                });
                context.Roles.Add(new ApplicationRole()
                {
                    Name = "Client",
                    Description = "Client User",
                    NormalizedName = "CLIENT"
                });

                context.SaveChanges();
            }
        }
    }
}
