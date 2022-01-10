using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Backend.Repositories.Interfaces;

namespace TodoApp.Backend.Repositories;

public class UsersRepo : IUsersRepo
{
  private readonly UserManager<IdentityUser<Guid>> _userManager;

  public UsersRepo(UserManager<IdentityUser<Guid>> userManager)
  {
    _userManager = userManager;
  }
  public async Task<List<IdentityUser<Guid>>> GetAllUsersAsync()
  {
    return await _userManager.Users.AsNoTracking().ToListAsync();
  }
}
