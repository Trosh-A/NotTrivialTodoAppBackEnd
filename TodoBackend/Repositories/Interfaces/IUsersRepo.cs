using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApp.Backend.Repositories.Interfaces;

public interface IUsersRepo
{
  Task<List<IdentityUser<Guid>>> GetAllUsersAsync();
}
