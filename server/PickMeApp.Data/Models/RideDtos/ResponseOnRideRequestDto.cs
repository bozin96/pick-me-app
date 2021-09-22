using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Models.RideDtos
{
    public class ResponseOnRideRequestDto
    {
        [Required]
        public Guid NotificationId { get; set; }

        public bool Accepted { get; set; }
    }
}
