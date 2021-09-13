using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideCheckInDto
    {
        [Required]
        public Guid RideId { get; set; }
        
        [Required]
        public int NumberOfPlaces { get; set; }
    }
}
