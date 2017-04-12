using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Data;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class EventController
    {
        private ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return this._context.Events.ToArray();
        }
        
        [HttpGet]
        [Route("next")]
        public IEnumerable<Event> GetNextEvents()
        {
            return this._context.Events.Where(d => d.ExpirationDate >= DateTime.Now.Date);
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody]Event value)
        {

            //Dao.Edit(value);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            //Dao.Delete(id);
        }
    }
}
