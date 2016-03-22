using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Repositories
{
    public abstract class SqlBaseRepository
    {
        protected string ConnectionString { get; set; }
    }
}
