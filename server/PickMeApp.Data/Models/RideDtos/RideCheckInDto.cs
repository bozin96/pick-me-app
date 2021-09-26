using System;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideCheckInDto
    {
        [Required]
        public Guid RideId { get; set; }
        
        [Required]
        public int NumberOfPassengers { get; set; }
    }
}
