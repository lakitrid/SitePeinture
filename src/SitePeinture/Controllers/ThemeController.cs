using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using SitePeinture.Dao;
using SitePeinture.Services;

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
        [Route("parents/{Id}")]
        public IEnumerable<Theme> GetParents([FromRoute]int Id)
        {
            ThemeServices services = new ThemeServices(Dao);

            return services.GetParents(Id);
        }

        [HttpPost]
        public void Post([FromBody] Theme theme)
        {
            ThemeServices services = new ThemeServices(Dao);

            services.Edit(theme);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute]int id)
        {
            ThemeServices services = new ThemeServices(Dao);

            services.Delete(id);
        }
    }
}
