using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BitirmeProjesi.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyf;
        public RequestController(ApplicationDbContext context, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.Request
               .Include(r => r.ApplicationUser)
               .Where(a => a.Check == false)
               .ToListAsync();
            return View(data);

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request? request)
        {
            if (request == null)
            {
                _notyf.Error("İzin Oluşturulamadı!");
                return View("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            request.ApplicationUserId = user.Id;
            _context.Add(request);
            await _context.SaveChangesAsync();
            _notyf.Success("İzin Oluşturuldu!");
            return View(request);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }
            RequestViewModel vm = (new RequestViewModel
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ApplicationUser = request.ApplicationUser,
                ApplicationUserId = request.ApplicationUserId,
                Approve = request.Approve,
                Check = request.Check
            });


            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Details(RequestViewModel? request)
        {
            if (request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }
            var item = _context.Request.Where(d => d.Id == request.Id).AsNoTracking().FirstOrDefault();
            if (item == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }
            item.Approve = request.Approve;
            item.Check = true;
            _context.Update(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _notyf.Success("İzin Güncellendi!");
            return RedirectToAction("Index", "Home");
        }
    }
}
