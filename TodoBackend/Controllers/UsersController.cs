using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Backend.Models;
using TodoApp.Backend.Services.JwtGenerator;

namespace TodoApp.Backend.Controllers;

[Route("api/")]
[AllowAnonymous]
[ApiController]
public class UsersController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;
  private readonly UserManager<IdentityUser<Guid>> _userManager;
  private readonly IJwtGenerator _jwtGenerator;

  public UsersController(
    ILogger<UsersController> logger,
    UserManager<IdentityUser<Guid>> userManager,
    IJwtGenerator jwtGenerator
    )
  {
    _logger = logger;
    _userManager = userManager;
    _jwtGenerator = jwtGenerator;
  }

  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync(RegistrationQueryModel registrationQuery)
  {
    if (registrationQuery is null)
    {
      return BadRequest();
    }
    var newUser = new IdentityUser<Guid>(registrationQuery.UserName);
    var createResult = await _userManager.CreateAsync(newUser, registrationQuery.Password);
    var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "user");
    if (createResult.Succeeded && addToRoleResult.Succeeded)
    {
      var claims = await _userManager.GetClaimsAsync(newUser);
      _logger.LogInformation($"Зарегистрирован {newUser.UserName}");
      return StatusCode(201, new
      {
        registrationQuery.UserName,
        Token = _jwtGenerator.CreateToken(newUser, claims)
      }); ;
    }
    else
    {
      foreach (var error in createResult.Errors)
      {
        ModelState.AddModelError(error.Code, error.Description);
      }
      foreach (var error in addToRoleResult.Errors)
      {
        ModelState.AddModelError(error.Code, error.Description);
      }
      return ValidationProblem(ModelState);
    }
  }

  [HttpPost("token")]
  public async Task<IActionResult> LoginAsync(LoginQueryModel loginQuery)
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
      _logger.LogInformation($"Выпущен токен для {user.UserName}");
      return Ok(new
      {
        user.UserName,
        Token = _jwtGenerator.CreateToken(user, claims),
      });
    }
    ModelState.AddModelError("", "Некорректный логин/пароль");
    return BadRequest(ModelState);
  }
}
