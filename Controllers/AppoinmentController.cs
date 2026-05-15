using Microsoft.AspNetCore.Mvc;

namespace Barber.Controllers;

public class AppoinmentController : Controller
{ 
    [HttpGet]
    public IActionResult Schedule() 
    { 
        return View();
    } 
    
    [HttpPost]
    public async Task<IActionResult> Schedule(DateTime date) 
    { 
        return View(); 
    }
     
}