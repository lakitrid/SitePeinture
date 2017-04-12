using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Dao;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class EventController
    {
        public DaoEvent Dao { get; set; }

        public EventController(DaoEvent dao)
        {
            this.Dao = Dao;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return Dao.GetAll().ToArray();
        }
        
        [HttpGet]
        [Route("next")]
        public IEnumerable<Event> GetNextEvents()
        {
            return Dao.GetAll().Where(d => d.ExpirationDate >= DateTime.Now.Date);
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody]Event value)
        {
            Dao.Edit(value);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            Dao.Delete(id);
        }
    }
}
