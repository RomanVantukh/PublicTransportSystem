using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Entities
{
    public class Route
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int Distance { get; set; }

        public int Price { get; set; }

        public int Duration { get; set; }

        public string Customer { get; set; }
    }
}
