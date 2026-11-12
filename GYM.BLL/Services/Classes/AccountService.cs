using GYM.BLL.Services.Interfaces;
using GYM.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GYM.BLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
        }

        public Task LogoutAsync() => _signInManager.SignOutAsync();
    }
}
