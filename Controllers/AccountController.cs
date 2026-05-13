using System.Security.Claims;
using Barber.Data;
using Barber.Models;
using Barber.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (!ModelState.IsValid) return View(user);
        
        var result = await _userService.RegisterUserAsync(user);

        if (result.Success)
        {
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(result.Field ,result.ErrorMessage);
        
        return View(user);
        
    }
    

    [HttpGet]
    public IActionResult Register(string email)
    {
        var NewUser = new User
        {
            FirstName = "",
            LastName = "",
            Phone = "",
            Email = email
        };
        return View(NewUser);
    }
    
    
    [HttpGet]
    public IActionResult Login() //SOLO MUESTRA EL FORMULARIO
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email) // es async y devuelve un task ya que es una operacion que lleva tiempo
                                                         // y no queremos que se trabe la app
    {
        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null) 
        {
            return RedirectToAction("Register",new {email = email});        
        }
            
        else             
        {
            var claims = new List<Claim>                //Lista de declaraciones. Datos sueltos sobre el usuario
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            
            var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);   //Agrupa todas las etiquetas
            ClaimsPrincipal principal = new ClaimsPrincipal(ClaimsIdentity);            //comando que manda ordenes al navegador. objeto final

            
            var properties = new AuthenticationProperties();
            properties.IsPersistent = true;     //Para que se guarden las cookies asi cuando se cierra el navegador la sesion sigue activa
            
           await HttpContext.SignInAsync
               (CookieAuthenticationDefaults.AuthenticationScheme, principal, properties); //Lo manda hacia el navegador
           
           return RedirectToAction("Index", "Home");                        //Cambiar variables


        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

}