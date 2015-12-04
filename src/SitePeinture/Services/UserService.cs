using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Services
{
    public class UserService
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        internal async Task<bool> Login(string login, string password)
        {
            var signInStatus = await this._signInManager.PasswordSignInAsync(login, password, true, false);
            if(signInStatus == SignInResult.Success)
            {
                return true;
            }

            return false;
        }

        internal void SignOut()
        {
            this._signInManager.SignOutAsync().Wait();
        }

        internal async void ChangePassword(PasswordUser user, string userId)
        {
            User connectedUser = await this._userManager.FindByNameAsync(userId);

            await this._userManager.ChangePasswordAsync(connectedUser, user.CurrentPassword, user.NewPassword);
        }
    }
}
