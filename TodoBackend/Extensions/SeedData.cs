using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using TodoApp.Backend.EFCore;

namespace TodoApp.Backend.Extensions;

public static class SeedData
{
  public static async Task AddSeedDataAsync(this WebApplication wa, Logger logger)
  {
    try
    {
      using var scope = wa.Services.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
      await DbInitializer.SeedUsersAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
      logger.Error(ex, "Stopped program because of seeding exception");
      throw;
    }
  }
}
