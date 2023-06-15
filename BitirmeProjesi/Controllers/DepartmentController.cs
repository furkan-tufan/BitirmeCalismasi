using AspNetCoreHero.ToastNotification.Abstractions;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BitirmeProjesi.Controllers
{
    [Authorize(Roles = "İnsan Kaynakları")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        public DepartmentController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Department.OrderBy(s => s.DepartmentName).ToListAsync();
            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string depName)
        {
            if (depName != null)
            {
                Department dep = new Department() { DepartmentName = depName };
                _context.Add(dep);
                await _context.SaveChangesAsync();
                _notyf.Success("Departman Eklendi");
            }
            return RedirectToAction("Index");
        }
    }
}