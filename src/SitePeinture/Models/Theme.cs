using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Theme
    {
        public decimal Id { get; set; }

        public string Title { get; set; }

        public decimal ParentId { get; set; }

        public string ParentTitle { get; set; }

        public string Description { get; set; }
    }
}
