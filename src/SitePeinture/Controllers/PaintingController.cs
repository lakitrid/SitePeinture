using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;

namespace SitePeinture.Controllers
{
    [Route("[controller]")]
    public class PaintingController : Controller
    {
        [FromServices]
        public DaoBase Dao { get; set; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return Dao.Get();            
        }

        ////// GET api/values/5
        ////[HttpGet("{id}")]
        ////public string Get(int id)
        ////{
        ////    return "value";
        ////}

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Painting value)
        {

        }

        ////// PUT api/values/5
        ////[HttpPut("{id}")]
        ////public void Put(int id, [FromBody]string value)
        ////{
        ////}

        ////// DELETE api/values/5
        ////[HttpDelete("{id}")]
        ////public void Delete(int id)
        ////{
        ////}
    }
}
