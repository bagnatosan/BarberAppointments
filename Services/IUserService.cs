namespace Barber.Services;
using Barber.Models;

public interface IUserService
{
    Task<(bool Success, string ErrorMessage, string? Field)> RegisterUserAsync(User user);
    
    Task<User?> GetUserByEmailAsync(string email);
}