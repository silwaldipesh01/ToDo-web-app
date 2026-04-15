using ToDo_App.Model.DTO;
using ToDo_App.Model;

namespace ToDo_App.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        bool ValidateUser(string username, string password); // Simplified for this example
    }
}