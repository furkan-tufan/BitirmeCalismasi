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

namespace BitirmeProjesi.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        public RequestController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
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

        public async Task<IActionResult> Approve(int? id)
        {
            if(id == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }
            var item=await _context.Request.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(item == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }
            item.Approve = true;
            item.Check = true;
            _notyf.Error("İzin Kabul Edildi!");
            return View("Index", "Home");
        }

        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }
            var item = await _context.Request.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (item == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }
            item.Approve = true;
            item.Check = true;
            _notyf.Error("İzin Kabul Edildi!");
            return View("Index", "Home");

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }

            var request = await _context.Request
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return View("Index", "Home");
            }

            return View(request);
        }

        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,ApplicationUserId,Approve")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserId);
            return View(request);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserId);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,ApplicationUserId,Approve")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(request);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserId);
            return View(request);
        }
    }
}
