using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;
using SitePeinture.Services;
using Microsoft.AspNet.Authorization;

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class ThemeController : Controller
    {
        [FromServices]
        public DaoTheme Dao { get; set; }

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
