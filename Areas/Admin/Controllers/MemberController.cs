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

        //[Authorize(Roles ="Admin")]
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
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(CreateMemberVM createMemberVM)
        {
            if (!ModelState.IsValid) return View();

            if (!createMemberVM.Image.CheckContent("image"))
            {
                ModelState.AddModelError("Image", "Enter the right format");
                return View(createMemberVM);
            }

            if (!createMemberVM.Image.ChecckLength(3000000))
            {
                ModelState.AddModelError("Image", "Enter the right size,less than 3mb");
                return View(createMemberVM);
            }
            Member member = new Member()
            {
                Name = createMemberVM.Name,
                Work = createMemberVM.Work,
                //Image = createMemberVM.Image.Upload(_env.WebRootPath, @"/Upload/Images/"),
            };
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Member");
        }

        //[Authorize(Roles ="Admin")]
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

        //[HttpPost]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(UpdateMemberVM updateMemberVM)
        {
            if (!ModelState.IsValid) return View();
            Member member = await _dbContext.Members.Where(x=>x.Id == updateMemberVM.Id).FirstOrDefaultAsync();
            if (member == null) throw new Exception(" Member can't be null! ");

            if(updateMemberVM.Image is not null)
            {
                if (!updateMemberVM.Image.CheckContent("image"))
                {
                    ModelState.AddModelError("Image", "Enter the right format");
                    return View(updateMemberVM);
                }
                if (!updateMemberVM.Image.ChecckLength(3000000))
                {
                    ModelState.AddModelError("Image", "Enter the right size,less than 3mb");
                    return View(updateMemberVM);
                }
            }

            member.ImgUrl = updateMemberVM.Image.Upload(_env.WebRootPath, @"/Upload/images/");

            member.Name=updateMemberVM.Name;
            member.Work=updateMemberVM.Work;
            member.Image = updateMemberVM.Image;

            _dbContext.Update(member);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Member"); 
        }

        //[Authorize(Roles ="Admin")]
        public  IActionResult Delete(int id)
        {
            Member member = _dbContext.Members.Where(x => x.Id == id).FirstOrDefault();
            _dbContext.Members.Remove(member);
            return RedirectToAction(nameof(Index), "Home");
        }
   }
}
