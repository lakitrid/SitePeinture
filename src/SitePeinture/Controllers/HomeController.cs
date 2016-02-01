using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    public class HomeController : Controller
    {
        [FromServices]
        public IConfiguration Configuration { get; set; }

        // GET: /<controller>/
        public IActionResult Index()
        {
#if DEBUG
            ViewBag.Version = new Random().Next().ToString();
#else
            ViewBag.Version = Configuration.GetSection("version").Value;
#endif

            ViewBag.Timeout = Configuration.Get<int>("timeout");
            return View();
        }
    }
}
