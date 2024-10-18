using Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class LogRepository : BaseRepository<Log>
    {
        public LogRepository(AppDbContext context) : base(context)
        {

        }
    }
}
