using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Backend.Models;
using TodoApp.Backend.Services.JwtGenerator;

namespace TodoApp.Backend.Services.UsersManagement;

public class UsersManagement : IUsersManagement
{
  private readonly UserManager<IdentityUser<Guid>> _userManager;
  private readonly IJwtGenerator _jwtGenerator;

  public UsersManagement(UserManager<IdentityUser<Guid>> userManager, IJwtGenerator jwtGenerator)
  {
    _userManager = userManager;
    _jwtGenerator = jwtGenerator;
  }
  public async Task<(string userName, string Token, IEnumerable<string> errors)> LoginAsync(LoginQueryModel loginQuery)
  {
    var user = await _userManager.FindByNameAsync(loginQuery.UserName);
    if (await _userManager.CheckPasswordAsync(user, loginQuery.Password))
    {
      var roles = await _userManager.GetRolesAsync(user);
      var claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
      }
      return (loginQuery.UserName, _jwtGenerator.CreateToken(user, claims), Enumerable.Empty<string>());
    }
    var errors = new List<string>() { "Некорректный логин/пароль" };
    return (String.Empty, String.Empty, errors);
  }

  public async Task<(string userName, string Token, IEnumerable<string> errors)> RegisterAsync(RegistrationQueryModel registrationQuery)
  {
    var newUser = new IdentityUser<Guid>(registrationQuery.UserName);
    var createResult = await _userManager.CreateAsync(newUser, registrationQuery.Password);
    var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "user");
    if (createResult.Succeeded && addToRoleResult.Succeeded)
    {
      var claims = await _userManager.GetClaimsAsync(newUser);
      return (registrationQuery.UserName, _jwtGenerator.CreateToken(newUser, claims), Enumerable.Empty<string>());
    }
    else
    {
      var errors = new List<string>();
      errors.AddRange(createResult.Errors.Select(e => e.Description));
      errors.AddRange(addToRoleResult.Errors.Select(e => e.Description));
      return (String.Empty, String.Empty, errors);
    }
  }
}
