using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanServices planServices;
        public PlanController(IPlanServices planServices)
        {
            this.planServices = planServices; 
        }
        public  async Task<IActionResult> Index(CancellationToken ct) => View(await planServices.GetPlansAsync(ct : ct));

        public async Task<IActionResult> Details(int id , CancellationToken ct) => View(await planServices.GetPlan(id , ct : ct));
        
        
        [HttpGet]         
        public  async  Task<IActionResult> Edit(int id  ,  CancellationToken ct )
        {
            var result  =  await planServices.GetPlan(id, ct: ct);

            if(result is null) return RedirectToAction("Index");

            return View(result); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlanView plan, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                await planServices.UpdateViewCreated(plan, ct);
                return RedirectToAction("Index");
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
                await planServices.CreatePlan(plan, ct);
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            await planServices.DeletePlan(id, ct);
            return RedirectToAction("Index");
        }
    }
}
