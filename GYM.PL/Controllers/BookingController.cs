using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace GYM.PL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _bookingService.GetManageableSessionsAsync(ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(new List<SessionViewModel>());
            }
            return View(result.Data);
        }

        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
        {
            var result = await _bookingService.GetSessionMembersAsync(id, true, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
        {
            var result = await _bookingService.GetSessionMembersAsync(id, false, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var membersResult = await _bookingService.GetAvailableMembersAsync(ct);
            if (!membersResult.IsSuccess)
            {
                TempData["ErrorMessage"] = "Failed to load members.";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id });
            }

            ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
            return View(new CreateBookingViewModel { SessionId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                var membersResult = await _bookingService.GetAvailableMembersAsync(ct);
                ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
                return View(model);
            }

            var result = await _bookingService.CreateBookingAsync(model, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                var membersResult = await _bookingService.GetAvailableMembersAsync(ct);
                ViewBag.Members = new SelectList(membersResult.Data.Select(x => new { x.Id, x.Name }), "Id", "Name");
                return View(model);
            }

            TempData["SuccessMessage"] = "Booking created successfully.";
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int MemberId, int SessionId, CancellationToken ct)
        {
            var result = await _bookingService.CancelBookingAsync(MemberId, SessionId, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Booking canceled successfully.";
            }

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attended(int MemberId, int SessionId, CancellationToken ct)
        {
            var result = await _bookingService.MarkAsAttendedAsync(MemberId, SessionId, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Member marked as attended.";
            }

            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = SessionId });
        }
    }
}
