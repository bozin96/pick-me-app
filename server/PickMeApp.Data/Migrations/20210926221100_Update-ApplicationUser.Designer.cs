﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using iComplyICO.Data;

namespace PickMeApp.Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210926221100_Update-ApplicationUser")]
    partial class UpdateApplicationUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<float>("AverageRate")
                        .HasColumnType("real");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MiddleName")
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<int>("NumberOfRates")
                        .HasColumnType("integer");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<byte[]>("UserAvatarPhoto")
                        .HasColumnType("bytea");

                    b.Property<string>("UserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<byte[]>("UserPhoto")
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Message.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastMessageTimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SecondUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FirstUserId");

                    b.HasIndex("SecondUserId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Message.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<string>("SendUserId")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SendUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Notification.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<string>("Header")
                        .HasColumnType("text");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("boolean");

                    b.Property<string>("Payload")
                        .HasColumnType("text");

                    b.Property<Guid>("RideId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("UserFromId")
                        .HasColumnType("text");

                    b.Property<string>("UserToId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RideId");

                    b.HasIndex("UserFromId");

                    b.HasIndex("UserToId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.PassengerOnRide", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DriverName")
                        .HasColumnType("text");

                    b.Property<string>("EndWaypoint")
                        .HasColumnType("text");

                    b.Property<string>("PassengerId")
                        .HasColumnType("text");

                    b.Property<int?>("Review")
                        .HasColumnType("integer");

                    b.Property<Guid>("RideId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("StartWaypoint")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PassengerId");

                    b.HasIndex("RideId");

                    b.ToTable("PassengerOnRides");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Invalidated")
                        .HasColumnType("boolean");

                    b.Property<string>("JwtId")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<bool>("Used")
                        .HasColumnType("boolean");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Ride", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DriverId")
                        .HasColumnType("text");

                    b.Property<int>("NumberOfFreeSeats")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfPassengers")
                        .HasColumnType("integer");

                    b.Property<bool>("PetFriendly")
                        .HasColumnType("boolean");

                    b.Property<string>("QueryField")
                        .HasColumnType("text");

                    b.Property<int>("RouteIndex")
                        .HasColumnType("integer");

                    b.Property<string>("RouteLegs")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Waypoints")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.ApplicationRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("ApplicationRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Message.Chat", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId");

                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Message.Message", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.Message.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "SendUser")
                        .WithMany()
                        .HasForeignKey("SendUserId");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Notification.Notification", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "UserFrom")
                        .WithMany()
                        .HasForeignKey("UserFromId");

                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "UserTo")
                        .WithMany()
                        .HasForeignKey("UserToId");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.PassengerOnRide", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "Passenger")
                        .WithMany("RidesAsPassenger")
                        .HasForeignKey("PassengerId");

                    b.HasOne("PickMeApp.Core.Models.Ride", "Ride")
                        .WithMany("Passengers")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PickMeApp.Core.Models.RefreshToken", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PickMeApp.Core.Models.Ride", b =>
                {
                    b.HasOne("PickMeApp.Core.Models.ApplicationUser", "Driver")
                        .WithMany("RidesAsDriver")
                        .HasForeignKey("DriverId");
                });
#pragma warning restore 612, 618
        }
    }
}
