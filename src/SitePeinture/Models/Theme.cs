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

        internal bool HasParent
        {
            get
            {
                if (this.ParentId == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

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
