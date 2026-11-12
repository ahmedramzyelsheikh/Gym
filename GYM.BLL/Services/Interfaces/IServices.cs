using GYM.BLL.Common;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GYM.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
    }

    public interface IAnalyticsService
    {
        Task<Result<AnalyticsViewModel>> GetDashboardAnalyticsAsync(CancellationToken ct = default);
    }

    public interface IAttachmentService
    {
        // Saves the file under wwwroot/Files/Images/{folderName} and returns the new file name.
        Task<Result<string>> UploadAsync(IFormFile file, string folderName, CancellationToken ct = default);
        void Delete(string? fileName, string folderName);
    }

    public interface IMemberService
    {
        Task<Result<IEnumerable<MemberView>>> GetAllMembersAsync(CancellationToken ct = default);
        Task<Result<MemberView>> GetMemberAsync(int id, CancellationToken ct = default);
        Task<Result> CreateMemberAsynce(CreateMemberViewModel model, CancellationToken ct = default);
        Task<Result<HealthRecordViewModel>> GetDetailsHealthRecord(int memberId, CancellationToken ct = default);
        Task<Result<MemberToUpdateViewModel>> GetMemberToEditAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default);
    }

    public interface ITrainerService
    {
        Task<Result<IEnumerable<TrainerView>>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<Result<TrainerView>> Details(int id, CancellationToken ct = default);
        Task<Result> AddTrainer(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<Result<EditTrainerViewModel>> GetTrainerToEdit(int id, CancellationToken ct = default);
        Task<Result> EditCompete(EditTrainerViewModel model, CancellationToken ct = default);
        Task<Result> Delete(int id, CancellationToken ct = default);
    }

    public interface IPlanServices
    {
        Task<Result<IEnumerable<PlanView>>> GetPlansAsync(CancellationToken ct = default);
        Task<Result<PlanView>> GetPlan(int id, CancellationToken ct = default);
        Task<Result> CreatePlan(PlanView model, CancellationToken ct = default);
        Task<Result> UpdateViewCreated(PlanView model, CancellationToken ct = default);
        Task<Result> ToggleActivationAsync(int id, CancellationToken ct = default);
        Task<Result> DeletePlan(int id, CancellationToken ct = default);
    }

    public interface ISessionService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsAsync(int id, CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToEditAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);
        Task<Result<IEnumerable<CategoryViewModel>>> GetCategoriesAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<TrainerView>>> GetTrainersAsync(CancellationToken ct = default);
    }

    public interface IMembershipService
    {
        Task<Result<IEnumerable<MemberShipViewModel>>> GetAllActiveMembershipsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<PlanView>>> GetAvailablePlansAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);
        Task<Result> CancelMembershipAsync(int memberId, CancellationToken ct = default);
    }

    public interface IBookingService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetManageableSessionsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetSessionMembersAsync(int sessionId, bool upcoming, CancellationToken ct = default);
        Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default);
        Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
