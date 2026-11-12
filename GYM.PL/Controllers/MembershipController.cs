using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace GYM.PL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _membershipService.GetAllActiveMembershipsAsync(ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(new List<MemberShipViewModel>());
            }
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var membersResult = await _membershipService.GetAvailableMembersAsync(ct);
            var plansResult = await _membershipService.GetAvailablePlansAsync(ct);

            ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
            ViewBag.Plans = new SelectList(plansResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");

            return View(new CreateMemberShipViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                var membersResult = await _membershipService.GetAvailableMembersAsync(ct);
                var plansResult = await _membershipService.GetAvailablePlansAsync(ct);

                ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
                ViewBag.Plans = new SelectList(plansResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");

                return View(model);
            }

            var result = await _membershipService.CreateMembershipAsync(model, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                var membersResult = await _membershipService.GetAvailableMembersAsync(ct);
                var plansResult = await _membershipService.GetAvailablePlansAsync(ct);

                ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
                ViewBag.Plans = new SelectList(plansResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");

                return View(model);
            }

            TempData["SuccessMessage"] = "Membership created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _membershipService.CancelMembershipAsync(id, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Membership canceled successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
