using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;
using Microsoft.AspNet.Authorization;
using SitePeinture.Services;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class PaintingController : Controller
    {
        [FromServices]
        public PaintingService PaintingService { get; set; }

        [FromServices]
        public DaoPainting Dao { get; set; }
        
        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return Dao.GetAll().ToArray();
        }

        [HttpGet]
        [Route("{id}")]
        public Painting Get(int id)
        {
            return Dao.GetAll().Where(e => e.Id == id).FirstOrDefault();
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
            PaintingService.Delete(id);
        }
    }
}
