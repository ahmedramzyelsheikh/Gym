using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Helpers
{
    internal static class UserHelper
    {
        // Email and phone are unique across members AND trainers (same GymUser table).
        public static async Task<string?> CheckDuplicateAsync(IUnitOfWork uow, string email, string phone, int? excludeId, CancellationToken ct)
        {
            var users = uow.Repository<GymUser>().Query();
            if (excludeId.HasValue) users = users.Where(u => u.Id != excludeId.Value);

            if (await users.AnyAsync(u => u.Email == email, ct)) return "This email is already used.";
            if (await users.AnyAsync(u => u.Phone == phone, ct)) return "This phone number is already used.";
            return null;
        }
    }
}
