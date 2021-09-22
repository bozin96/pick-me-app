using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Helpers
{
    public class MyRideResourceParameters : ResourceParameters
    {
        public string DriverId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
