using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using SitePeinture.Models;

namespace Models
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext()
        {

        }

        
    }
}