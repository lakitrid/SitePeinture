using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitePeinture.Models;
using SitePeinture.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class UserController : Controller
    {
        public UserService UserService { get; set; }

        public UserController(UserService userService)
        {
            this.UserService = userService;
        }

        [HttpGet, Route("isAuth")]
        public bool IsAuth()
        {
            if(this.User == null)
            {
                return false;
            }

            if(!this.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return true;            
        }

        [HttpGet, Route("signOut")]
        public void SignOut()
        {
            this.UserService.SignOut();
        }

        [HttpPost]
        [Route("login")]
        public async Task<bool> Post([FromBody]LoginUser user)
        {
            try
            {
                return await this.UserService.Login(user.Login, user.Password);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost, Route("change")]
        [Authorize]
        public async Task<List<string>> ChangePassword([FromBody]PasswordUser user)
        {
            List<string> result = await this.UserService.ChangePassword(user, this.User.Identity.Name);
            if(result.Count > 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return result;
        }
    }
}
