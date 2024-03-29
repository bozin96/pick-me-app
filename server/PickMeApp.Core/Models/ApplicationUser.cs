﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(150)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(150)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public byte[] UserPhoto { get; set; }

        public byte[] UserAvatarPhoto { get; set; }

        public float AverageRate { get; set; }

        public int NumberOfRates { get; set; }

        public List<Ride> RidesAsDriver { get; set; }

        public List<PassengerOnRide> RidesAsPassenger { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }

        public string Description { get; set; }
    }

    public static class ApplicationUserRole
    {
        public const string Client = "Client";

        public const string Admin = "Admin";
    }
}
