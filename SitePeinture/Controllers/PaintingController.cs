using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Dao;
using SitePeinture.Models;
using SitePeinture.Services;
using System.Collections.Generic;
using System.Linq;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class PaintingController : Controller
    {
        public PaintingService PaintingService { get; set; }
        
        public DaoPainting Dao { get; set; }

        public PaintingController(PaintingService paintingService, DaoPainting dao)
        {
            this.PaintingService = paintingService;
            this.Dao = dao;
        }

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
