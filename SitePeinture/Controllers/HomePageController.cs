using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/home")]
    public class HomePageController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public HomePageController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

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
