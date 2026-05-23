using Barber.Data;
using Barber.Models;
using Barber.Services;
using Microsoft.AspNetCore.Mvc;

namespace Barber.Controllers;

public class AppointmentController : Controller
{ 
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    [HttpGet]
    public IActionResult Schedule() 
    { 
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots(int hairdresserId, string date)
    {
        var availableSlots = await _appointmentService.GetAvailableSlots(hairdresserId, date);
        return Json(availableSlots);
    }
    
    [HttpPost]
    public async Task<IActionResult> Schedule(DateTime date) 
    { 
        return View(); 
    }
     
}