using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entity
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime LogTimeUTC { get; set; }
        public string LogMessage { get; set; } = string.Empty;
    }
}
