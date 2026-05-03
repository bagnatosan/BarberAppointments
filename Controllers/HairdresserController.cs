using Barber.Data;
using Microsoft.AspNetCore.Mvc;

namespace Barber.Controllers;

public class HairdresserController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public HairdresserController(ApplicationDbContext context)
    {
        _context = context; 
    }

    public IActionResult Index()    //interface of action result
    {
        var hairdresser = _context.Hairdressers.ToList();
        return View(hairdresser);
    }
    
}