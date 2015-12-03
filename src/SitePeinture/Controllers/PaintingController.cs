using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;
using Microsoft.AspNet.Authorization;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class PaintingController : Controller
    {
        [FromServices]
        public DaoPainting Dao { get; set; }
        
        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return Dao.GetAll().ToArray();            
        }

        [HttpPost]
        [Authorize]
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

        [HttpGet]
        [Route("theme/{id}")]
        public IEnumerable<Painting> GetPaintingByThemeId([FromRoute]int id)
        {
            return Dao.GetAll().Where(e => e.ThemeId.Equals(id)).ToArray();
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
