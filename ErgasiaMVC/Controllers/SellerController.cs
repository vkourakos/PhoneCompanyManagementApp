using ErgasiaMVC.Data;
using ErgasiaMVC.Models;
using ErgasiaMVC.ViewModels.Seller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ErgasiaMVC.Controllers;

[Authorize(Roles = "Seller")]
public class SellerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public SellerController(
        ApplicationDbContext context,
        UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _context.Clients
            .Include(c => c.User)
            .Include(c => c.Phone)
            .ToListAsync();
        var vm = new SellerIndexVm
        {
            Clients = clients
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> CreateClient()
    {
        var programs = await _context.Programs
            .Select(p => new SelectListItem
            {
                Value = p.ProgramName,
                Text = p.ProgramName
            })
            .ToListAsync();

        var model = new CreateClientVm
        {
            Programs = programs
        };

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateClient(CreateClientVm model)
    {
        if (!ModelState.IsValid)
        {
            model.Programs = await _context.Programs
                .Select(p => new SelectListItem
                {
                    Value = p.ProgramName,
                    Text = p.ProgramName
                })
                .ToListAsync();

            return View(model);
        }

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

            model.Programs = await _context.Programs
                .Select(p => new SelectListItem
                {
                    Value = p.ProgramName,
                    Text = p.ProgramName
                })
                .ToListAsync();
            return View(model);
        }

        var phone = new Phone
        {
            PhoneNumber = model.PhoneNumber,
            ProgramName = model.ProgramName
        };

        _context.Phones.Add(phone);
        await _context.SaveChangesAsync();

        await _userManager.AddToRoleAsync(user, "Client");

        var client = new Client
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            Afm = model.Afm,
            PhoneNumber = phone.PhoneNumber,
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Seller");
    }

    [HttpGet]
    public async Task<IActionResult> IssueBill(string clientId)
    {
        var client = await _context.Clients
            .Include(c => c.Phone)
            .ThenInclude(p => p.Program)
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null)
            return NotFound();

        var programCharge = client.Phone?.Program?.Charge;

        if (programCharge <= 0 || programCharge == null)
            return BadRequest();

        var bill = new Bill
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = client.PhoneNumber,
            Costs = programCharge.Value,
        };

        var call1 = new Call
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Call to New York",
            CallerNumber = client.PhoneNumber,
            CalleeNumber = "6923456789"
        };
        var call2 = new Call
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Call to California",
            CallerNumber = "6923456789",
            CalleeNumber = client.PhoneNumber
        };

        bill.Calls.Add(call1);
        bill.Calls.Add(call2);

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Bill issued successfully for client {client.User?.FirstName} {client.User?.LastName}.";

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> ChangeProgram(string clientId)
    {
        var client = await _context.Clients
            .Include(c => c.Phone)
            .ThenInclude(p => p.Program)
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null)
            return NotFound();

        var programs = await _context.Programs
            .Select(p => new SelectListItem
            {
                Value = p.ProgramName,
                Text = p.ProgramName
            })
            .ToListAsync();

        var model = new ChangeProgramVm
        {
            ClientId = client.Id,
            PhoneNumber = client.PhoneNumber,
            CurrentProgram = client.Phone?.ProgramName!,
            Programs = programs
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeProgram(ChangeProgramVm changeProgramVm)
    {
        if (!ModelState.IsValid)
        {
            changeProgramVm.Programs = await _context.Programs
                .Select(p => new SelectListItem
                {
                    Value = p.ProgramName,
                    Text = p.ProgramName
                })
                .ToListAsync();
            return View(changeProgramVm);
        }

        var client = await _context.Clients
            .Include(c => c.Phone)
            .FirstOrDefaultAsync(c => c.Id == changeProgramVm.ClientId);

        if (client == null)
            return NotFound();

        var phone = client.Phone;
        if (phone == null)
            return NotFound();

        phone.ProgramName = changeProgramVm.NewProgram;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Client's program has been successfully updated.";
        return RedirectToAction("Index", "Seller");
    }

}

