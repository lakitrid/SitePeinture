using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Services;
using SitePeinture.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class UserController : Controller
    {
        [FromServices]
        public UserService UserService { get; set; }

        [HttpPost]
        [Route("login")]
        public async Task<bool> Post([FromBody]LoginUser user)
        {
            try
            {
                return await this.UserService.Login(user.Login, user.Password);
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
