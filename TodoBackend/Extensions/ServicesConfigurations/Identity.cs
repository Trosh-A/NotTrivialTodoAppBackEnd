using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Backend.EFCore;
using TodoApp.Backend.Models;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class Identity
{
  public static IdentityBuilder ConfigureIdentity(
    this IServiceCollection isc)
  {

    return isc
      .AddIdentityCore<IdentityUser<Guid>>(x =>
      {
        x.Password.RequiredLength = 5;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequireLowercase = false;
        x.Password.RequireUppercase = false;
        x.Password.RequireDigit = false;
      })
      .AddRoles<IdentityRole<Guid>>()
      .AddSignInManager<SignInManager<IdentityUser<Guid>>>()
      .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
      /*
      .AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(x =>
    {
      x.Password.RequiredLength = userOptions.MinimalPasswordLength;
      x.Password.RequireNonAlphanumeric = false;
      x.Password.RequireLowercase = false;
      x.Password.RequireUppercase = false;
      x.Password.RequireDigit = false;
    })*/
      .AddEntityFrameworkStores<AppDbContext>();
  }
}
