using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TodoApp.Backend.Constants;

namespace TodoApp.Backend.EFCore;

public static class DbInitializer
{
  public static async Task SeedUsersAsync(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
  {
    await SeedRolesAsync(roleManager);
    await SeedUsersAsync(userManager);
  }
  private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
  {
    var seedRoles = new[] { RolesConstants.adminRole, RolesConstants.userRole };
    foreach (var seedRole in seedRoles)
    {
      if (!await roleManager.RoleExistsAsync(seedRole))
      {
        await roleManager.CreateAsync(new IdentityRole<Guid>(seedRole));
      }
    }
  }
  private static async Task SeedUsersAsync(UserManager<IdentityUser<Guid>> userManager)
  {
    string andrewUserName = "Andrew";
    if (await userManager.FindByNameAsync(andrewUserName) is null)
    {
      var andrewUser = new IdentityUser<Guid>(andrewUserName);
      await userManager.CreateAsync(andrewUser, "54321");
      await userManager.AddToRoleAsync(andrewUser, RolesConstants.adminRole);
    }

    string alexUserName = "Alex";
    if (await userManager.FindByNameAsync(alexUserName) is null)
    {
      var alexUser = new IdentityUser<Guid>(alexUserName);
      await userManager.CreateAsync(alexUser, "12345");
      await userManager.AddToRoleAsync(alexUser, RolesConstants.userRole);
    }
  }
}
