using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Column("Modification")]
        public DateTime ModificationDate { get; set; }

        [Column("Expiration")]
        public DateTime ExpirationDate { get; set; }

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
