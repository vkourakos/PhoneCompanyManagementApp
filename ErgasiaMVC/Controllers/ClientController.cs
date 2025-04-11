using ErgasiaMVC.Data;
using ErgasiaMVC.Models;
using ErgasiaMVC.Models.Enums;
using ErgasiaMVC.ViewModels.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErgasiaMVC.Controllers;

[Authorize(Roles = "Client")]
public class ClientController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ClientController(
        ApplicationDbContext context,
        UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var client = await _context.Clients
            .Include(c => c.User)
            .Include(c => c.Phone)
            .ThenInclude(p => p.Program)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null)
            return NotFound();

        var bills = await _context.Bills
            .Where(b => b.PhoneNumber == client!.PhoneNumber)
            .ToListAsync();


        var model = new ClientIndexVm
        {
            ClientId = client.Id,
            PhoneNumber = client.PhoneNumber,
            Bills = bills
        };

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> ViewBill(string id)
    {
        var bill = await _context.Bills
            .Include(b => b.Phone)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null)
            return NotFound();

        var model = new ViewBillVm
        {
            Id = bill.Id,
            PhoneNumber = bill.PhoneNumber,
            Costs = bill.Costs,
            Status = bill.Status,
            ProgramName = bill.Phone?.ProgramName!
        };

        return View(model);
    }


    public async Task<IActionResult> ViewCallHistory(string phoneNumber)
    {
        var calls = await _context.Calls
            .Where(c => c.CallerNumber == phoneNumber || c.CalleeNumber == phoneNumber)
            .ToListAsync();

        return View(calls);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayBill(string billId)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(b => b.Id == billId);

        if (bill == null)
        {
            return NotFound();
        }

        if (bill.Status == BillStatus.Paid)
        {
            TempData["Error"] = "This bill has already been paid.";
            return RedirectToAction("Index");
        }

        bill.Status = BillStatus.Paid;

        _context.Update(bill);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Bill has been successfully paid.";
        return RedirectToAction("Index");
    }

}

