using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Painting
    {
        public string Title { get; set; }

        public string Theme { get; set; }

        public string Filename { get; set; }

        public string Data { get; set; }

        public decimal Id { get; set; }

        public decimal ThemeId { get; set; }

        public string ThemeTitle { get; set; }

        public string Description { get; set; }

        public bool OnSlider { get; set; }

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
