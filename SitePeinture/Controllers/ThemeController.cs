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
    public class ThemeController : Controller
    {
        public DaoTheme Dao { get; set; }

        public ThemeController(DaoTheme dao)
        {
            this.Dao = dao;
        }

        [HttpGet]
        public IEnumerable<Theme> Get()
        {
            ThemeServices services = new ThemeServices(Dao);

            return services.GetAll().ToArray();
        }

        [HttpGet]
        [Route("{Id}")]
        public Theme GetWithId([FromRoute] int Id)
        {
            ThemeServices services = new ThemeServices(Dao);

            return services.GetAll().Where(e => e.Id.Equals(Id)).FirstOrDefault();
        }

        [HttpGet]
        [Route("subthemes/{Id}")]
        public IEnumerable<Theme> GetSubThemeWithId([FromRoute] int Id)
        {
            ThemeServices services = new ThemeServices(Dao);

            return services.GetAll().Where(e => e.ParentId.Equals(Id)).ToArray();
        }

        [HttpGet]
        [Route("parents")]
        public IEnumerable<Theme> GetAllParents()
        {
            return Dao.GetAll().Where(e => e.HasParent == false).ToArray();
        }

        [HttpGet]
        [Route("parents/{Id}")]
        public IEnumerable<Theme> GetParents([FromRoute]int Id)
        {
            ThemeServices services = new ThemeServices(Dao);

            return services.GetParents(Id);
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody] Theme theme)
        {
            ThemeServices services = new ThemeServices(Dao);

            services.Edit(theme);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            ThemeServices services = new ThemeServices(Dao);

            services.Delete(id);
        }
    }
}
