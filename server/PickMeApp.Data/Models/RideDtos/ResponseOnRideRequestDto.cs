using System;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models.RideDtos
{
    public class ResponseOnRideRequestDto
    {
        [Required]
        public Guid NotificationId { get; set; }

        public bool Accepted { get; set; }
    }
}
