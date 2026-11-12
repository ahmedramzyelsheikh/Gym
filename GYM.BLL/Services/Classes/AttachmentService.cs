using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.Services.Classes
{
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
