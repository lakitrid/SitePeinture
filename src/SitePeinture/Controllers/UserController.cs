using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        [FromServices]
        public UserService UserService { get; set; }

        [HttpPost]
        [Route("login")]
        public async Task<bool> Post([FromBody]string login, [FromBody]string password)
        {
            return await this.UserService.Login(login, password);
        }
    }
}
