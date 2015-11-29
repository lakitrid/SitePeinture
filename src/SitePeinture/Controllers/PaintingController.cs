using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class PaintingController : Controller
    {
        [FromServices]
        public DaoPainting Dao { get; set; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return Dao.GetAll().ToArray();            
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
            Dao.Edit(value);
        }
        
        [HttpGet]
        [Route("slider")]
        public IEnumerable<Painting> GetSliderPaints()
        {
            return Dao.GetAll().Where(e => e.OnSlider).ToArray();
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            Dao.Delete(id);
        }
    }
}
