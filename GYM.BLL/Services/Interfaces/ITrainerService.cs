using GYM.BLL.Common;
using GYM.BLL.ViewModels.Trainers;

namespace GYM.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<Result<IEnumerable<TrainerView>>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<Result<TrainerView>> Details(int id, CancellationToken ct = default);
        Task<Result> AddTrainer(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<Result<EditTrainerViewModel>> GetTrainerToEdit(int id, CancellationToken ct = default);
        Task<Result> EditCompete(EditTrainerViewModel model, CancellationToken ct = default);
        Task<Result> Delete(int id, CancellationToken ct = default);
    }
}
