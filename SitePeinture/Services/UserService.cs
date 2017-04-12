using Microsoft.AspNetCore.Identity;
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
            await this.CheckIfAdminExists();

            var signInStatus = await this._signInManager.PasswordSignInAsync(login, password, true, false);
            if (signInStatus == SignInResult.Success)
            {
                return true;
            }

            return false;
        }

        private async Task CheckIfAdminExists()
        {
            User user = await this._userManager.FindByNameAsync("admin");
            if (user == null)
            {
                // add editor user
                user = new User
                {
                    UserName = "admin"
                };

                await this._userManager.CreateAsync(user, "BasicPwd24!");
            }
        }

        internal void SignOut()
        {
            this._signInManager.SignOutAsync().Wait();
        }

        internal async Task<List<string>> ChangePassword(PasswordUser user, string userId)
        {
            List<string> errors = new List<string>();
            User connectedUser = await this._userManager.FindByNameAsync(userId);

            try
            {
                IdentityResult result = await this._userManager.ChangePasswordAsync(connectedUser, user.CurrentPassword, user.NewPassword);
                if (result != IdentityResult.Success)
                {
                    errors.AddRange(result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception exc)
            {
                errors.Add(exc.Message);
            }

            return errors;
        }
    }
}
