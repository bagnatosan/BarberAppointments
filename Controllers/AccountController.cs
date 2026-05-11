using System.Security.Claims;
using Barber.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barber.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Login() //Sirve solo para mostra el archivo .cshtml en el formulario
    {
        return View();
    }

    public async Task<IActionResult> Login(string email) // es async y devuelve un task ya que es una operacion que lleva tiempo y no queremos que se trabe la app
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) //NO EXISTE EL USUARIO EN LA BASE DE DATOS
        {
            RedirectToAction("Register","Home");        //Cambiar variables
        }
            
        else              //SI EXISTE EL USUARIO EN LA BASE DE DATOS
        {
            var claims = new List<Claim>                //Lista de declaraciones. Datos sueltos sobre el usuario
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            
            var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);   //Agrupa todas las etiqueras
            ClaimsPrincipal principal = new ClaimsPrincipal(ClaimsIdentity);            //comando que manda ordenes al navegador. objeto final

            
            var properties = new AuthenticationProperties();
            properties.IsPersistent = true;     //Para que se guarden las cookies asi cuando se cierra el navegador la sesion sigue activa
            
           await HttpContext.SignInAsync
               (CookieAuthenticationDefaults.AuthenticationScheme, principal, properties); //Lo manda hacia el navegador
           
           return RedirectToAction("Index", "Home");                        //Cambiar variables


        }

        return View();
    }

}