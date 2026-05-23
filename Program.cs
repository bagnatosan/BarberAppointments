using Barber.Data;
using Barber.Models;
using Barber.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite("Data Source=Barber.db"));
builder.Services.AddScoped<IUserService , UserService>();
builder.Services.AddScoped<IAppointmentService , AppointmentService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // A dónde mandarlo si no está logueado
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "BarberiaCookie"; // El nombre del "sello"
    options.ExpireTimeSpan = TimeSpan.FromDays( 90 ); // Duración (Persistencia)
    options.SlidingExpiration = true; // Si la usa, se renueva el tiempo

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); //cookies

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();