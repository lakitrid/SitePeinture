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
    public class ThemeController : Controller
    {
        [FromServices]
        public DaoTheme Dao { get; set; }

        [HttpGet]
        public IEnumerable<Theme> Get()
        {
            return Dao.GetAll().ToArray();
        }

        [HttpGet]
        [Route("parents/{Id}")]
        public IEnumerable<Theme> GetParents([FromRoute]decimal Id)
        {
            return Dao.GetAll().Where(e => e.HasParent == false && e.Id != Id).ToArray();
        }

        [HttpPost]
        public void Post([FromBody] Theme theme)
        {
            Dao.Edit(theme);
        }
    }
}
