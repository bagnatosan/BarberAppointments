using System.Diagnostics;
using System.Security.Claims;
using Barber.Data;
using Microsoft.AspNetCore.Mvc;
using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public HomeController(ApplicationDbContext context)
    {
        _context = context; 
    }
    
    
        
    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var appointments = await _context.Appointments
            .Where(u => u.UserId == userId && u.Date > DateTime.Now)
            .Select( u => u.Date)
            .ToListAsync();
        
        return View(appointments);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}