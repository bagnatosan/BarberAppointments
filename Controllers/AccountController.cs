using System.Security.Claims;
using Barber.Models;
using Barber.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

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

        ModelState.AddModelError(result.Field ?? string.Empty /*Si esto es nulo, usa este otro*/
            ,result.ErrorMessage);
        
        return View(user);
        
    }
    

    [HttpGet]
    public IActionResult Register(string email)
    {
        var newUser = new User
        {
            FirstName = "",
            LastName = "",
            Phone = "",
            Email = email
        };
        return View(newUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetRole(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null)
            return Json("NotFound");
        else
        {
            var role = user.Role;
            return Json(role);
        }
        
    }
    
    [HttpGet]
    public IActionResult Login() //SOLO MUESTRA EL FORMULARIO
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email , string? password) // es async y devuelve un task ya que es una operacion que lleva tiempo// y no queremos que se trabe la app
    {
        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null) 
        {
            return RedirectToAction("Register",new {email});   
        }
        
        else if (user.Password != password && user.Role != "Customer")
        {
            ModelState.AddModelError("Password","Contraseña incorrecta");
            return View();
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
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);   //Agrupa todas las etiquetas
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);            //comando que manda ordenes al navegador. objeto final

            
            var properties = new AuthenticationProperties();
            properties.IsPersistent = true;     //Para que se guarden las cookies asi cuando se cierra el navegador la sesion sigue activa
            
           await HttpContext.SignInAsync
               (CookieAuthenticationDefaults.AuthenticationScheme, principal, properties); //Lo manda hacia el navegador
           
           return RedirectToAction("Index", "Home");                        //Cambiar variables


        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

}