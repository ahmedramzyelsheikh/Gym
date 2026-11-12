using Microsoft.AspNetCore.Identity;

namespace GYM.BLL.Services.Interfaces.Account
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
