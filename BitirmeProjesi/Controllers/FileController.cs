using AspNetCoreHero.ToastNotification.Abstractions;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BitirmeProjesi.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly INotyfService _notyf;
        private readonly UserManager<ApplicationUser> _userManager;
        public FileController(ApplicationDbContext context, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_notyf = notyf;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                _notyf.Error("Dosya Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            var file = await _context.FileModel.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null)
            {
                _notyf.Error("Dosya Bulunamadı!");
                return RedirectToAction("HomePage", "Home");
            }
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }
	}
}