using GYM.BLL.ViewModels;
using GYM.DAL.Repositories.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GYM.PL.Controllers
{
    public class MemberController : Controller
    {

        readonly IMemberService _member;
        public MemberController(IMemberService _member)
        {
            this._member = _member; 
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var  result = await  _member.GetAllMembersAsync(ct);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> Create( CreateMemberViewModel member, CancellationToken ct)
        {
            if(ModelState.IsValid)
            {
                await _member.CreateMemberAsynce(member, ct); 
                return RedirectToAction("Index");
            }

            return View(member); 
                
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _member.GetMemberAsync(id, ct);
            if (result is null) return RedirectToAction("Index");

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _member.GetMemberToEditAsync(id, ct);
            if (result is null) return RedirectToAction("Index");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberToUpdateViewModel member, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                await _member.UpdateMemberAsync(member, ct);
                return RedirectToAction("Index");
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
