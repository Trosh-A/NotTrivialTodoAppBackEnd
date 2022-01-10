using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Backend.Models;

namespace TodoApp.Backend.Services.UsersManagement;

public interface IUsersManagement
{
  Task<(string userName, string Token, IEnumerable<string> errors)> RegisterAsync(RegistrationQueryModel registrationQuery);
  Task<(string userName, string Token, IEnumerable<string> errors)> LoginAsync(LoginQueryModel loginQuery);
}
