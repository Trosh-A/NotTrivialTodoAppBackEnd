using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Backend.Models.UserModel;
using TodoApp.Backend.Repositories.Interfaces;

namespace TodoApp.Backend.Controllers;

[Route("api/admin/")]
[Authorize(Roles = "admin")]
[ApiController]
public class DashboardController : ControllerBase
{
  private readonly ILogger<DashboardController> _logger;
  private readonly ITodosRepo _todoRepo;
  private readonly IUsersRepo _usersRepo;
  private readonly IMapper _mapper;
  private readonly UserManager<IdentityUser<Guid>> _userManager;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DashboardController(
    ILogger<DashboardController> logger,
    ITodosRepo todoRepo,
    IUsersRepo usersRepo,
    IMapper mapper,
    UserManager<IdentityUser<Guid>> userManager,
    IHttpContextAccessor httpContextAccessor)
  {
    _logger = logger;
    _todoRepo = todoRepo;
    _usersRepo = usersRepo;
    _mapper = mapper;
    _userManager = userManager;
    _httpContextAccessor = httpContextAccessor;
  }

  // GET: api/admin/todos-counts
  [HttpGet("todos-counts")]
  public async Task<ActionResult<int>> GetTodosCountAsync()
  {
    _logger.LogInformation($"Админ \"{_httpContextAccessor.HttpContext?.User.Identity?.Name ?? "(Имя не определено)"}\" запросил количество всех \"todos\"");
    return Ok(await _todoRepo.GetAllTodosCountAsync());
  }
  // GET: api/admin/users
  [HttpGet("users")]
  public async Task<ActionResult<List<UserReadDto>>> GetUsersAsync()
  {
    var users = await _usersRepo.GetAllUsersAsync();
    var usersReadDtos = _mapper.Map<List<UserReadDto>>(users);
    foreach (var userReadDto in usersReadDtos)
    {
      var user = await _userManager.FindByIdAsync(userReadDto.Guid.ToString());
      userReadDto.Roles = await _userManager.GetRolesAsync(user);
    }
    _logger.LogInformation($"Админ \"{_httpContextAccessor.HttpContext?.User.Identity?.Name ?? "(Имя не определено)"}\" запросил список всех пользователей");
    return Ok(usersReadDtos);
  }
}
