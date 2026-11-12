using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GYM.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _sessionService.GetAllSessionsAsync(ct);
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (!result.IsSuccess) return NotFound();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct);
            return View(new CreateSessionViewModel());
        }

        [HttpPost]
      
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.CreateSessionAsync(model, ct);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Session created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToEditAsync(id, ct);
            if (!result.IsSuccess) return NotFound();

            await PopulateDropDownsAsync(ct);
            return View(result.Data);
        }

        [HttpPost]
        
        public async Task<IActionResult> Edit(UpdateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.UpdateSessionAsync(model, ct);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Session updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (!result.IsSuccess) return NotFound();
            return View(result.Data);
        }

        [HttpPost, ActionName("Delete")]
    
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            var categoriesResult = await _sessionService.GetCategoriesAsync(ct);
            if (categoriesResult.IsSuccess)
            {
                ViewBag.Categories = new SelectList(categoriesResult.Data.Select(c => new { c.Id, c.Name }), "Id", "Name");
            }

            var trainersResult = await _sessionService.GetTrainersAsync(ct);
            if (trainersResult.IsSuccess)
            {
                ViewBag.Trainers = new SelectList(trainersResult.Data.Select(t => new { t.Id, t.Name }), "Id", "Name");
            }
        }
    }
}
