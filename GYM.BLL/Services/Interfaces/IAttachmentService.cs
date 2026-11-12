using GYM.BLL.Common;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.Services.Interfaces
{
    public interface IAttachmentService
    {
        // Saves the file under wwwroot/Files/Images/{folderName} and returns the new file name.
        Task<Result<string>> UploadAsync(IFormFile file, string folderName, CancellationToken ct = default);
        void Delete(string? fileName, string folderName);
    }
}
