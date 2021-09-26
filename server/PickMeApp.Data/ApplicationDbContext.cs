using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Message;
using PickMeApp.Core.Models.Notification;
using System.Collections.Generic;

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

        public DbSet<PassengerOnRide> PassengerOnRides { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Chat> Chats { get; set; }

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

            builder.Entity<PassengerOnRide>()
                .HasOne(por => por.Passenger)
                .WithMany(p => p.RidesAsPassenger)
                .HasForeignKey(por => por.PassengerId);
            builder.Entity<PassengerOnRide>()
                .HasOne(por => por.Ride)
                .WithMany(r => r.Passengers)
                .HasForeignKey(por => por.RideId);

        }

    }
}