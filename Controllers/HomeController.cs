using System.Diagnostics;
using System.Security.Claims;
using Barber.Data;
using Barber.Dto;
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
            .AsNoTracking()
            .Where(u => u.UserId == userId && u.Date >= DateTime.Now && u.IsCanceled == false)
            .Select( u => u.Date)
            .ToListAsync();
        
        var hasActiveRecurrence = await _context.RecurrentSchedules
            .AnyAsync(u => u.UserId == userId && u.IsActive);
        
        var hasAppointmentRegular = await  _context.Appointments
            .AnyAsync( a => a.RecurrentSchedulesId == null && a.IsCanceled == false 
            && a.UserId == userId && a.Date > DateTime.Now);


        var viewModel = new RecurrenceAvailabilityDto()
        {
            Dates = appointments,
            HasActiveRecurrence = hasActiveRecurrence,
            HasAppointmentRegular =  hasAppointmentRegular
        };
            
        
        return View(viewModel);
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