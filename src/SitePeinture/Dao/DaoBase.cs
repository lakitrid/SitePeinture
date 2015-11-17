using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SitePeinture.Models;

namespace SitePeinture.Dao
{
    public class DaoBase
    {
        public IConfiguration configuration { get; set; }

        public DaoBase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

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
    }
}
