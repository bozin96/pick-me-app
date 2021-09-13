using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models
{
    public class RouteInfo
    {
        public float TotalPrice { get; set; }

        public float TotalDistance { get; set; }

        public int TotalTime { get; set; }

        public int StartTime { get; set; }

        public int RouteIndex { get; set; }

        public List<Waypoint> Waypoints { get; set; }
    }
}
