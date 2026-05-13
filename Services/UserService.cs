using Barber.Data;
using Barber.Models;
using Microsoft.EntityFrameworkCore;

namespace Barber.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string ErrorMessage, string? Field)> RegisterUserAsync(User user)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync
            (u => u.Email == user.Email || u.Phone == user.Phone);
        
        if (userExists != null)
        {
            if (userExists.Email == user.Email)
                return (false, "El email ya esta en uso", "Email");             //mail
                return (false, "El numero de telefono ya existe" , "Phone");  //telefono
        }
        
        user.Phone = $"54911{user.Phone}";
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return (true, "" , "");
        
    }
    
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
    }
}