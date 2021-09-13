using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace iComplyICO.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationRole> AppRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Ride> Rides { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ride>()
                   .Property(r => r.Waypoints)
                   .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<Waypoint>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            builder.Entity<Ride>()
                   .Property(r => r.RouteLegs)
                   .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<RouteLeg>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


            //builder.Entity<Ride>()
            //    .OwnsOne(p => p.RouteInfo, p =>
            //    {
            //        p.Property(pp => pp.TotalPrice)
            //            .HasColumnName("TotalPrice");
            //        p.Property(pp => pp.TotalDistance)
            //            .HasColumnName("TotalDistance");
            //        p.Property(pp => pp.TotalTime)
            //            .HasColumnName("TotalTime");
            //        p.Property(pp => pp.StartTime)
            //            .HasColumnName("StartTime");
            //        p.Property(pp => pp.RouteIndex)
            //            .HasColumnName("RouteIndex");
            //        p.Property(pp => pp.Waypoints)
            //            .HasColumnName("Waypoints")
            //            .HasConversion(
            //                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            //                v => JsonConvert.DeserializeObject<List<Waypoint>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            //    });
        }

    }
}