using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Dao
{
    public class DaoEvent : DaoBase
    {
        public DaoEvent(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Event> GetAll()
        {
            return new List<Event>();
        }
    }
}
