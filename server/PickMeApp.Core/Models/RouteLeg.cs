using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models
{
    public class RouteLeg
    {
        public float StartLongitude { get; set; }

        public float StartLatitude { get; set; }

        public float EndLongitude { get; set; }

        public float EndLatitude { get; set; }

        public float Price { get; set; }

        public float Distance { get; set; } // distance in meters.

        public int Time { get; set; } // time duration in seconds.

        public int NumberOfFreeSpaces { get; set; }
    }
}
