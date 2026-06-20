using GYM.BLL.ViewModels;
using GYM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GYM.BLL.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace GYM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _analyticsService.GetDashboardAnalyticsAsync(ct);
            return View(result.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
