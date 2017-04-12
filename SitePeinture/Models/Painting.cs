using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Painting
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ThemeId { get; set; }

        public Theme Theme { get; set; }

        public string Filename { get; set; }

        public string Description { get; set; }

        public bool OnSlider { get; set; }

        public int Price { get; set; }

        public bool Available { get; set; }

        [NotMapped]
        public string Data { get; set; }

        [NotMapped]
        internal bool IsNew
        {
            get
            {
                if (this.Id == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
