using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Entities
{
    public class Driver
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string BusNumber { get; set; }

        public string RouteNumber { get; set; }

        public string Customer { get; set; }
    }
}
