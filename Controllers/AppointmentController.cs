using Barber.Services;
using Barber.Dto;
using Barber.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> Schedule() 
    {
        var hairdressers = await _appointmentService.GetHairdressers();
        var haircuts = await _appointmentService.GetHaircutsAsync();
        var viewModel = new AppointmentDto();
        viewModel.Hairdressers = hairdressers;
        viewModel.Haircuts = haircuts;
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

    public async Task<IActionResult> GetHaircutsAsync()
    {
        var haircuts = await _appointmentService.GetHaircutsAsync();
        var result = haircuts.Select(e => new { e.Id, e.Name, e.Price });
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> CancelAppointment(DateTime date, int userId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isCanceled = await _appointmentService.CancelAppointmentAsync(date, userId);

        if (isCanceled) return Ok(new { message = "El turno fue cancelado correctamente" });

        return NotFound(new { message = "No se encontró ningún turno activo con esa fecha" });
    }
}