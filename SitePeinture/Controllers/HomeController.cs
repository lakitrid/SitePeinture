using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public HomeController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
#if DEBUG
            ViewBag.Version = new Random().Next().ToString();
#else
            ViewBag.Version = Configuration.GetSection("version").Value;
#endif

            ViewBag.Timeout = Configuration.GetValue<int>("timeout");
            return View();
        }
    }
}
