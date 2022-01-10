using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Backend.Models;
using TodoApp.Backend.Services.JwtGenerator;
using TodoApp.Backend.Services.UsersManagement;

namespace TodoApp.Backend.Controllers;

[Route("api/")]
[AllowAnonymous]
[ApiController]
public class UsersController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;
  private readonly UserManager<IdentityUser<Guid>> _userManager;
  private readonly IJwtGenerator _jwtGenerator;
  private readonly IUsersManagement _usersManagement;

  public UsersController(
    ILogger<UsersController> logger,
    UserManager<IdentityUser<Guid>> userManager,
    IJwtGenerator jwtGenerator,
    IUsersManagement usersManagement
    )
  {
    _logger = logger;
    _userManager = userManager;
    _jwtGenerator = jwtGenerator;
    _usersManagement = usersManagement;
  }

  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync(RegistrationQueryModel registrationQuery)
  {
    if (registrationQuery is null)
    {
      return BadRequest();
    }
    var (userName, token, errors) = await _usersManagement.RegisterAsync(registrationQuery);
    if (errors.Any())
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError("", error);
      }
      return ValidationProblem(ModelState);
    }
    _logger.LogInformation($"Зарегистрирован пользователь \"{userName}\" и выпущен токен");
    return StatusCode(201, new { userName, token });
  }

  [HttpPost("token")]
  public async Task<IActionResult> LoginAsync(LoginQueryModel loginQuery)
  {
    if (loginQuery is null)
    {
      return BadRequest();
    }
    var (userName, token, errors) = await _usersManagement.LoginAsync(loginQuery);
    if (errors.Any())
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError("", error);
      }
      return ValidationProblem(ModelState);
    }
    _logger.LogInformation($"Выпущен токен для {userName}");
    return Ok(new { userName, token });
  }
}
