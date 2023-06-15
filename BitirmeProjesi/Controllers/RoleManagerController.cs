using AspNetCoreHero.ToastNotification.Abstractions;
using BitirmeProjesi.Data;
using BitirmeProjesi.Data.Migrations;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "İnsan Kaynakları")]
public class RoleManagerController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyf;
    public RoleManagerController(INotyfService notyf, ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
        _context = context;
        _notyf = notyf;
    }
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.Include(b => b.Department).ToListAsync();
        ViewData["DepartmentList"] = new SelectList(_context.Set<Department>(), "DepartmentId", "DepartmentName");
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string roleName, int department)
    {
        if (roleName != null)
        {
            await _roleManager.CreateAsync(new ApplicationRole(roleName.Trim())
            {
                DepartmentId = department
            });
            _notyf.Success("Pozisyon Eklendi");
        }
        return RedirectToAction("Index");
    }
}