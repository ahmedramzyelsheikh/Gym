using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices; 
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _planServices.GetPlansAsync(ct);
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _planServices.GetPlan(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");
            return View(result.Data);
        }

        [HttpGet]         
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _planServices.GetPlan(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");
            return View(result.Data); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlanView plan, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _planServices.UpdateViewCreated(plan, ct);
                if (result.IsSuccess) return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(plan);
        }

        [HttpGet]
        public IActionResult Create() => View(new PlanView());

        [HttpPost]
        public async Task<IActionResult> Create(PlanView plan, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _planServices.CreatePlan(plan, ct);
                if (result.IsSuccess) return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(plan);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            await _planServices.DeletePlan(id, ct);
            return RedirectToAction("Index");
        }
    }
}
