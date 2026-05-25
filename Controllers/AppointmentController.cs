using Barber.Services;
using Barber.Dto;
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
    public async Task<IActionResult> Schedule() 
    {
        var hairdressers = await _appointmentService.GetHairdressers();
        var viewModel = new AppointmentDto();
        viewModel.Hairdressers = hairdressers;
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots(int hairdresserId, string date)
    {
        var availableSlots = await _appointmentService.GetAvailableSlotsAsync(hairdresserId, date);
        return Json(availableSlots);
    }
    
    [HttpPost]
    public async Task<IActionResult> Schedule(DateTime date) 
    { 
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> Insert ([FromBody]AppointmentDto appointment)
    {
        var success = await _appointmentService.InsertAppointmentAsync(appointment);

        if (success)
            return Ok();            //le manda un estado 200 al JavaScript
        
        else
            return BadRequest("No se pudo agendar el turno correctamente");
        
    }

   
  
     
}