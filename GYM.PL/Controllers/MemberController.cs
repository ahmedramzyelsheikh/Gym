using GYM.BLL.ViewModels;
using GYM.DAL.Repositories.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GYM.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _member;
        
        public MemberController(IMemberService member)
        {
            _member = member; 
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _member.GetAllMembersAsync(ct);
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel member, CancellationToken ct)
        {
            if(ModelState.IsValid)
            {
                var result = await _member.CreateMemberAsynce(member, ct); 
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            return View(member); 
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _member.GetMemberAsync(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");

            return View(result.Data);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _member.GetDetailsHealthRecord(id, ct);
            var model = result.IsSuccess ? result.Data : new HealthRecordViewModel { Note = result.ErrorMessage };

            TempData["Message"] = "Successfully loaded Health Record for the member.";
            ViewBag.Status = "Active";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _member.GetMemberToEditAsync(id, ct);
            if (!result.IsSuccess) return RedirectToAction("Index");

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberToUpdateViewModel member, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _member.UpdateMemberAsync(member, ct);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            return View(member);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            await _member.DeleteMemberAsync(id, ct);
            return RedirectToAction("Index");
        }
    }
}
