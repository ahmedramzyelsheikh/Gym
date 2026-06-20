using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct);
            return View(new CreateSessionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.CreateSessionAsync(model, ct);
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Session created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"] = "Failed to create session.";
            }
            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var model = await _sessionService.GetSessionToEditAsync(id, ct);
            if (model == null) return NotFound();

            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                await _sessionService.UpdateSessionAsync(model, ct);
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            await _sessionService.DeleteSessionAsync(id, ct);
            TempData["SuccessMessage"] = "Session deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            var categories = await _sessionService.GetCategoriesAsync(ct);
            ViewBag.Categories = new SelectList(categories.Select(c => new { c.Id, c.Name }), "Id", "Name");

            var trainers = await _sessionService.GetTrainersAsync(ct);
            ViewBag.Trainers = new SelectList(trainers.Select(t => new { t.Id, t.Name }), "Id", "Name");
        }
    }
}
