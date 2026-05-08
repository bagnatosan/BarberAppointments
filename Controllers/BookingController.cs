using Barber.Data;
using Microsoft.AspNetCore.Mvc;

namespace Barber.Controllers;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    
    // GET
    public IActionResult SelectHairdresser()
    {
        var hairdresser = _context.Hairdressers.ToList();
        return View(hairdresser);
    }
}