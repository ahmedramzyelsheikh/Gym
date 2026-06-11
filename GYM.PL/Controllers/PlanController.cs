
using GYM.BLL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GYM.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository _PlanRepository;
        public PlanController(IPlanRepository _PlanRepository)
        {
            this._PlanRepository = _PlanRepository; 
        }
        public  async Task<IActionResult> Index(CancellationToken ct) => View(await _PlanRepository.GetAllAync(ct : ct));

        public async Task<IActionResult> Details(int id) => View(await _PlanRepository.GetById(id));
       

    }
}
