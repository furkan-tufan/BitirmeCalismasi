using AspNetCoreHero.ToastNotification.Abstractions;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BitirmeProjesi.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyf;
        public CourseController(ApplicationDbContext context, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _notyf = notyf;
        }

        [Authorize]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (_context.Course == null)
            {
                _notyf.Error("Eğitim Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            var courses = await _context.Course.Where(c => c.EmployeeCourses.Any(i => i.EmployeeId == user.Id)).ToListAsync();
            return View(courses);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (_context.Course == null)
            {
                _notyf.Error("Eğitim Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            var courses = await _context.Course.ToListAsync();
            return View(courses);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Education(int? id)
        {
            if (id == null)
            {
                _notyf.Error("Eğitim Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                _notyf.Error("Eğitim Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            ViewBag.Workers = _context.Users.Include(p => p.EmployeeCourses).Where(a => a.IsActive == true).OrderBy(s => s.Name).ToList();
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "İnsan Kaynakları")]
        public IActionResult Education(int id, string[] userId)
        {
            var course = _context.Course.Include(p => p.EmployeeCourses).FirstOrDefault(i => i.Id == id);
            if (course != null)
            {
                course.EmployeeCourses = userId.Select(emloyeeId => new EmployeeCourses()
                {
                    CourseId = id,
                    EmployeeId = emloyeeId
                }).ToList();
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Create([Bind("Id,Name,Application,Link")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "İnsan Kaynakları")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Application,Link")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}