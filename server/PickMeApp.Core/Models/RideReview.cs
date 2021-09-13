using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models
{
    public class RideReview : BaseEntity
    {
        public Guid RideId { get; set; }

        public Ride Ride { get; set; }

        public Guid ReviewerId { get; set; }

        public ApplicationUser Reviewer { get; set; }

        public Guid DriverId { get; set; }

        public ApplicationUser MyProperty { get; set; }

    }
}
