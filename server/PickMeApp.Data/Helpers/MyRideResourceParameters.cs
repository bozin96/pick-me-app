using System;

namespace PickMeApp.Application.Helpers
{
    public class MyRideResourceParameters : ResourceParameters
    {
        public string DriverId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
