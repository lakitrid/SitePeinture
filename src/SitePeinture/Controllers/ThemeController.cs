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
        public IEnumerable<Theme> Index()
        {
            return Dao.GetAll().ToArray();
        }
    }
}
