using ErgasiaMVC.Data;
using ErgasiaMVC.Models;
using ErgasiaMVC.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErgasiaMVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var programs = await _context.Programs.ToListAsync();
        var vm = new AdminIndexVm
        {
            Programs = programs
        };

        return View(programs);
    }

    [HttpGet]
    public IActionResult CreateSeller()
    {
        var vm = new CreateSellerVm();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSeller(CreateSellerVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Seller");

        var seller = new Seller
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id
        };

        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult CreateProgram()
    {
        var vm = new CreateProgramVm();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProgram(CreateProgramVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var program = new Models.Program
        {
            ProgramName = model.ProgramName,
            Benefits = model.Benefits,
            Charge = model.Charge
        };

        _context.Programs.Add(program);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditProgram(string programName)
    {
        var program = await _context.Programs
            .FirstOrDefaultAsync(p => p.ProgramName == programName);

        if (program == null)
        {
            return NotFound();
        }

        var model = new EditProgramVm
        {
            ProgramName = program.ProgramName,
            Benefits = program.Benefits,
            Charge = program.Charge
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProgram(EditProgramVm model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var program = await _context.Programs
            .FirstOrDefaultAsync(p => p.ProgramName == model.ProgramName);

        if (program == null)
        {
            return NotFound();
        }

        program.Benefits = model.Benefits;
        program.Charge = model.Charge;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}

