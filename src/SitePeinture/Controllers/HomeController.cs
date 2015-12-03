using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class HomeController : Controller
    {
        [FromServices]
        public IConfiguration Configuration { get; set; }

        [HttpGet]
        public string Get()
        {
            string homeArticleFileName = Configuration.GetSection("homeArticle").Value;

            if (System.IO.File.Exists(homeArticleFileName))
            {
                return System.IO.File.ReadAllText(homeArticleFileName);
            }

            return string.Empty;
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody]string homeArticle)
        {
            string homeArticleFile = Configuration.GetSection("homeArticle").Value;

            System.IO.File.WriteAllText(homeArticleFile, homeArticle);
        }
    }
}
