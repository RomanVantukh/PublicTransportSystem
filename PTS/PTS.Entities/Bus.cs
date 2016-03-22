using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Entities
{
    public class Bus
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Model { get; set; }

        public string RouteNumber { get; set; }

        public string Customer { get; set; }
    }
}
