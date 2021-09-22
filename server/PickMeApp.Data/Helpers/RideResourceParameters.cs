using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Helpers
{
    public class RideResourceParameters : ResourceParameters
    {
        [Required]
        public float StartLongitude { get; set; }

        [Required]
        public float StartLatitude { get; set; }

        [Required]
        public float EndLongitude { get; set; }

        [Required]
        public float EndLatitude { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int NumberOfPassengers { get; set; }

        public float Langitude { get; set; }

        public float Longitude { get; set; }
    }
}
