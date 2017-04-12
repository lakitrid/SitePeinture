using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Theme
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ParentId { get; set; }

        public Theme ParentTheme { get; set; }

        public string Description { get; set; }

        public bool WithText { get; set; }

        [NotMapped]
        public bool HasChildrenTheme { get; set; }

        [NotMapped]
        public bool HasChildrenPainting { get; set; }

        [NotMapped]
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
