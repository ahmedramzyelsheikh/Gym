using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _uow;

        public AnalyticsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<AnalyticsViewModel>> GetDashboardAnalyticsAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var sessions = _uow.Repository<Session>().Query();

            var model = new AnalyticsViewModel
            {
                TotalMembers = await _uow.Repository<Member>().Query().CountAsync(ct),
                ActiveMembers = await _uow.Repository<Membership>().Query()
                                    .Where(m => m.EndDate > now)
                                    .Select(m => m.MemberId).Distinct().CountAsync(ct),
                TotalTrainers = await _uow.Repository<Trainer>().Query().CountAsync(ct),
                UpcomingSessions = await sessions.CountAsync(s => s.StartDate > now, ct),
                OngoingSessions = await sessions.CountAsync(s => s.StartDate <= now && s.EndDate >= now, ct),
                CompletedSessions = await sessions.CountAsync(s => s.EndDate < now, ct)
            };

            return Result<AnalyticsViewModel>.Success(model);
        }
    }

    public class AttachmentService : IAttachmentService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private readonly IWebHostEnvironment _env;

        public AttachmentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<Result<string>> UploadAsync(IFormFile file, string folderName, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                return Result<string>.Failure("Please choose a file to upload.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return Result<string>.Failure("Only JPG, PNG and GIF images are allowed.");

            if (file.Length > MaxFileSize)
                return Result<string>.Failure("The image must be 5 MB or smaller.");

            var folderPath = GetFolderPath(folderName);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            await using var stream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
            await file.CopyToAsync(stream, ct);

            return Result<string>.Success(fileName);
        }

        public void Delete(string? fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            // Path.GetFileName blocks paths like "../../appsettings.json".
            var path = Path.Combine(GetFolderPath(folderName), Path.GetFileName(fileName));
            if (File.Exists(path)) File.Delete(path);
        }

        private string GetFolderPath(string folderName)
            => Path.Combine(_env.WebRootPath, "Files", "Images", folderName);
    }
}
