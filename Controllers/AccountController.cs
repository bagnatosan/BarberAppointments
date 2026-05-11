using System.Security.Claims;
using Barber.Data;
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
            RedirectToAction()
        }
            
        else              //SI EXISTE EL USUARIO EN LA BASE DE DATOS
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            
            var ClaimsIdentity = new ClaimsIdentity(claims, "Login");
            
            ClaimsPrincipal principal = new ClaimsPrincipal(ClaimsIdentity);
            
            
        }
            
        return View();
    }

}