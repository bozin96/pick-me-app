using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
