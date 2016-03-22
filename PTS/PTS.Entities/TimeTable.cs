using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Entities
{
    public class TimeTable
    {
        public int Id { get; set; }

        public string RouteNumber { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public int Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }
    }
}
