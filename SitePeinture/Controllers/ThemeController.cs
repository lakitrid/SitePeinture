using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Models;
using SitePeinture.Services;
using System.Collections.Generic;
using System.Linq;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class ThemeController : Controller
    {
        private ThemeServices _themeService;

        public ThemeController(ThemeServices themeService)
        {
            this._themeService = themeService;
        }

        [HttpGet]
        public IEnumerable<Theme> Get()
        {
            return this._themeService.GetAll().ToArray();
        }

        [HttpGet]
        [Route("{Id}")]
        public Theme GetWithId([FromRoute] int Id)
        {
            return this._themeService.GetAll().Where(e => e.Id.Equals(Id)).FirstOrDefault();
        }

        [HttpGet]
        [Route("subthemes/{Id}")]
        public IEnumerable<Theme> GetSubThemeWithId([FromRoute] int Id)
        {
            return this._themeService.GetAll().Where(e => e.ParentId.Equals(Id)).ToArray();
        }

        [HttpGet]
        [Route("parents")]
        public IEnumerable<Theme> GetAllParents()
        {
            return this._themeService.GetAll().Where(e => e.HasParent == false).ToArray();
        }

        [HttpGet]
        [Route("parents/{Id}")]
        public IEnumerable<Theme> GetParents([FromRoute]int Id)
        {
            return this._themeService.GetParents(Id);
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody] Theme theme)
        {
            this._themeService.Edit(theme);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            this._themeService.Delete(id);
        }
    }
}
