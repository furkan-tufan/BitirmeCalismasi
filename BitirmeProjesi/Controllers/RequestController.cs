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

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Index()
        {
            var data = await _context.Request
               .Include(r => r.ApplicationUser)
               .Where(a => a.Check == false)
               .ToListAsync();
            return View(data);

        }

        [Authorize]
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var data = await _context.Request
               .Include(r => r.ApplicationUser)
               .ToListAsync();
            return View(data);

        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Request? request)
        {
            if (request == null)
            {
                _notyf.Error("İzin Oluşturulamadı!");
                return View("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            request.ApplicationUserId = user.Id;

            foreach (var file in request.Files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileModel
                {
                    CreatedOn = time,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    RequestId = request.Id
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                request.FileList.Add(fileModel);
            }

            _context.Add(request);
            await _context.SaveChangesAsync();
            _notyf.Success("İzin Oluşturuldu!");
            return View(request);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }
            var request = await _context.Request.Include(a => a.ApplicationUser).Where(b => b.Id == id).FirstOrDefaultAsync();
            if (request == null)
            {
                _notyf.Error("İzin Bulunamadı!");
                return RedirectToAction("Index", "Home");
            }

            ViewData["User"] = request.ApplicationUser;
            RequestViewModel vm = (new RequestViewModel
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ApplicationUser = request.ApplicationUser,
                ApplicationUserId = request.ApplicationUserId,
                Approve = request.Approve,
                Check = request.Check,
                Files = request.Files,
                FileList = request.FileList
            });
            var files = _context.FileModel.Where(a => a.RequestId == id).ToList();
            ViewBag.Files = files;
            return View(vm);
        }

        [Authorize(Roles = "İnsan Kaynakları")]
        [HttpPost]
        public async Task<IActionResult> Details(RequestViewModel? request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
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


            TimeSpan difference = (TimeSpan)(item.EndDate - item.StartDate);
            int days = (int)difference.TotalDays;
            if (item.Approve)
            {
                user.Permission -= days;
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }

            _context.Update(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _notyf.Success("İzin Güncellendi!");
            return RedirectToAction("Index", "Home");
        }
    }
}
