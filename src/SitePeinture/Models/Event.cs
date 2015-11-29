using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ModificationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

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
