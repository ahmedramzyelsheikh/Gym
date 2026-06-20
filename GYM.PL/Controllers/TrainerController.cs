using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GYM.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainers;

        public TrainerController(ITrainerService trainers)
        {
            _trainers = trainers;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _trainers.GetAllTrainersAsync(ct); 
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct) => View(new CreateTrainerViewModel());

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel createTrainerViewModel, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainers.AddTrainer(createTrainerViewModel, ct);
                if (result.IsSuccess) return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _trainers.Details(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");
            return View(result.Data);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct = default) => View();
    
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            await _trainers.Delete(id, ct); 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await _trainers.GetTrainerToEdit(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");
            return View(result.Data); 
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirmed(EditTrainerViewModel updatetrainer, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainers.EditCompete(updatetrainer, ct);
                if (result.IsSuccess) return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return RedirectToAction("Index"); 
        }
    }
}
