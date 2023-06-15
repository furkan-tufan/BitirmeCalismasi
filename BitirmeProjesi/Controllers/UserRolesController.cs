using Bitirme.Models;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "İnsan Kaynakları")]
public class UserRolesController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    public UserRolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userRolesViewModel = new List<UserViewModel>();
        foreach (ApplicationUser user in users)
        {
            var thisViewModel = new UserViewModel();
            thisViewModel.UserId = user.Id;
            thisViewModel.Email = user.Email;
            thisViewModel.Salary = user.Salary;
            thisViewModel.Permission = user.Permission;
            thisViewModel.IsActive = user.IsActive;
            thisViewModel.StartDate = user.StartDate;
            thisViewModel.EndDate = user.EndDate;
            thisViewModel.Name = user.Name;
            thisViewModel.Roles = await GetUserRoles(user);
            userRolesViewModel.Add(thisViewModel);
        }
        return View(userRolesViewModel);
    }
    private async Task<List<string>> GetUserRoles(ApplicationUser user)
    {
        return new List<string>(await _userManager.GetRolesAsync(user));
    }
    public async Task<IActionResult> Manage(string userId)
    {
        ViewBag.userId = userId;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("NotFound");
        }
        ViewBag.UserName = user.UserName;
        var model = new List<UserRolesViewModel>();
        foreach (var role in _roleManager.Roles)
        {
            var userRolesViewModel = new UserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            model.Add(userRolesViewModel);
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Manage(List<UserRolesViewModel> model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return View();
        }
        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user existing roles");
            return View(model);
        }
        result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot add selected roles to user");
            return View(model);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(string userId)
    {
        if (userId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }
        var thisViewModel = new UserViewModel();
        thisViewModel.UserId = user.Id;
        thisViewModel.Email = user.Email;
        thisViewModel.PhoneNumber = user.PhoneNumber;
        thisViewModel.Salary = user.Salary;
        thisViewModel.Permission = user.Permission;
        thisViewModel.IsActive = user.IsActive;
        thisViewModel.StartDate = user.StartDate;
        thisViewModel.EndDate = user.EndDate;
        thisViewModel.Name = user.Name;
        return View(thisViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserViewModel userView)
    {
        var user = await _userManager.FindByIdAsync(userView.UserId);
        user.Name = userView.Name;
        user.Email = userView.Email;
        user.Salary = userView.Salary;
        user.Permission = userView.Permission;
        user.IsActive = userView.IsActive;
        user.EndDate = userView.EndDate;
        user.StartDate = userView.StartDate;
        user.PhoneNumber = userView.PhoneNumber;
        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public ActionResult ExportToExcel()
    {
        System.Data.DataTable table = GetData();
        string fileName = "Çalışan Listesi.xlsx";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(table);
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }

    private System.Data.DataTable GetData()
    {
        System.Data.DataTable table = new System.Data.DataTable();
        table.TableName = "Çalışan Listesi";
        table.Columns.Add("Çalışan İsmi");
        table.Columns.Add("Çalışan E-Postası");
        var data = _userManager.Users.ToList();
        for (int i = 0; i < data.Count(); i++)
        {
            table.Rows.Add(data[i].Name, data[i].Email);
        }
        table.AcceptChanges();
        return table;
    }
}