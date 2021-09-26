using System;
using System.ComponentModel.DataAnnotations;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideReviewDto
    {

        [Required]
        public Guid Id { get; set; }

        [Range(1,5)]
        [Required]
        public int Rate { get; set; }
    }
}
