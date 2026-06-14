using Barber.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barber.Controllers;

[Authorize(Roles = "hairdresser")]
public class HairdresserController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public HairdresserController(ApplicationDbContext context)
    {
        _context = context; 
    }

    public IActionResult Dashboard()    //interface of action result
    {
        return View();
    }
    
}