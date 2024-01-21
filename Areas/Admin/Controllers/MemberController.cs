using FinalLastExam.Areas.Admin.ViewModels;
using FinalLastExam.DAL;
using FinalLastExam.Helpers;
using FinalLastExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;

namespace FinalLastExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class MemberController:Controller
    {
        
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public MemberController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Member> members = await _dbContext.Members.ToListAsync();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateMemberVM createMemberVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createMemberVM);
            }

            if (!createMemberVM.Image.CheckLength(3000000))
            {
                ModelState.AddModelError("Image", " you can't upload more than 2 mb ");
                return View();
            }

            Member member = new Member()
            {
                Name = createMemberVM.Name,
                Work = createMemberVM.Work,
                ImgUrl = createMemberVM.Image.Upload(_env.WebRootPath, @"/Upload/Images/")
            };
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if(id<=0) return View();
            Member member = await _dbContext.Members.Where(x => x.Id == id).FirstOrDefaultAsync();
            UpdateMemberVM updateMemberVM = new UpdateMemberVM()
            {
                Name=member.Name,
                Work=member.Work,
                ImgUrl = member.ImgUrl,
            };
            return View(updateMemberVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateMemberVM updateMemberVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (updateMemberVM.Id <= 0) throw new Exception("Id can't be less than zero");
            var member = _dbContext.Members.FirstOrDefault<Member>(x => x.Id == updateMemberVM.Id);
            if (member == null) throw new Exception("Fruit can't be null");


            member.Name = updateMemberVM.Name;
            member.Work = updateMemberVM.Work;
            member.ImgUrl = updateMemberVM.Image.Upload(_env.WebRootPath, @"/Upload/Images");
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Delete(int id)
            {
                Member member = await _dbContext.Members.Where(x => x.Id == id).FirstOrDefaultAsync();
                 _dbContext.Members.Remove(member);
             await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
            }
   }
}
