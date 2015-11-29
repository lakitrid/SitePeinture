using Microsoft.AspNet.Mvc;
using SitePeinture.Dao;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class EventController
    {
        [FromServices]
        public DaoEvent Dao { get; set; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return Dao.GetAll().ToArray();
        }

        [HttpPost]
        public void Post([FromBody]Event value)
        {
            Dao.Edit(value);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            Dao.Delete(id);
        }
    }
}
