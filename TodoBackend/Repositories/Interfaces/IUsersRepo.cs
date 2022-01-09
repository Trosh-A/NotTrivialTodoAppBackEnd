using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TodoApp.Backend.Repositories.Interfaces;

public interface IUsersRepo
{
  Task<List<IdentityUser<Guid>>> GetAllUsersAsync();
}
