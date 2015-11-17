using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;

namespace SitePeinture.Controllers
{
    [Route("[controller]")]
    public class PaintingController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Painting> Get()
        {
            return new Painting[]
            {
                new Painting
                {
                    Title = "Tableau 1",
                    Theme = "animaux",
                    Filename = "4elementeau.jpg",
                },
                new Painting
                {
                    Title = "Tableau 2",
                    Theme = "animaux",
                    Filename = "4elementfeu.jpg",
                },
                new Painting
                {
                    Title = "Tableau 3",
                    Theme = "animaux",
                    Filename = "4elementterre.jpg",
                },
                new Painting
                {
                    Title = "Tableau 4",
                    Theme = "animaux",
                    Filename = "irissilk.jpg",
                }
            };
        }

        ////// GET api/values/5
        ////[HttpGet("{id}")]
        ////public string Get(int id)
        ////{
        ////    return "value";
        ////}

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Painting value)
        {

        }

        ////// PUT api/values/5
        ////[HttpPut("{id}")]
        ////public void Put(int id, [FromBody]string value)
        ////{
        ////}

        ////// DELETE api/values/5
        ////[HttpDelete("{id}")]
        ////public void Delete(int id)
        ////{
        ////}
    }
}
