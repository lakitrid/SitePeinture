using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Models;
using SitePeinture.Services;
using System.Collections.Generic;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class PaintingController : Controller
    {
        private PaintingService _paintingService;

        public PaintingController(PaintingService paintingService)
        {
            this._paintingService = paintingService;
        }

        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return this._paintingService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Painting Get(int id)
        {
            return this._paintingService.GetById(id);
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody]Painting value)
        {
            this._paintingService.Edit(value);
        }
        
        [HttpGet]
        [Route("slider")]
        public IEnumerable<Painting> GetSliderPaints()
        {
            return this._paintingService.GetSliderPaints();
        }

        [HttpGet]
        [Route("theme/{id}")]
        public IEnumerable<Painting> GetPaintingByThemeId([FromRoute]int id)
        {
            return this._paintingService.GetPaintingByThemeId(id);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            _paintingService.Delete(id);
        }
    }
}
