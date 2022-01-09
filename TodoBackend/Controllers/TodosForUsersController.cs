using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Backend.Models.TodoModel;
using TodoApp.Backend.Repositories.Interfaces;

namespace TodoApp.Backend.Controllers;


[Route("api/todos")]
[Authorize(Roles = "user")]
[ApiController]
public class TodosForUsersController : ControllerBase
{
  private readonly ITodosRepo _todoRepo;
  private readonly ILogger<TodosForUsersController> _logger;
  private readonly IValidator<Todo> _todoValidator;
  private readonly IMapper _mapper;

  public TodosForUsersController(
    ITodosRepo todoRepo,
    IMapper mapper,
    ILogger<TodosForUsersController> logger,
    IValidator<Todo> todoValidator
    )
  {
    _todoRepo = todoRepo;
    _mapper = mapper;
    _logger = logger;
    _todoValidator = todoValidator;
  }

  // GET: api/todos
  [HttpGet]
  public async Task<ActionResult<IEnumerable<TodoReadDTO>>> GetTodosAsync()
  {
    _logger.LogDebug("Запрошен список всех \"todos\"");
    var todos = await _todoRepo.GetAllTodosAuthAsync();
    return Ok(_mapper.Map<IEnumerable<TodoReadDTO>>(todos));
  }

  // GET: api/todos/{id}
  [HttpGet("{id:guid}")]
  public async Task<ActionResult<TodoReadDTO>> GetTodoByIdAsync(Guid id)
  {
    var todo = await _todoRepo.GetTodoByGuidAuthAsync(id);
    if (todo is not null)
    {
      _logger.LogDebug($"Запрошен \"todo\" с id: {id}");
      return Ok(_mapper.Map<TodoReadDTO>(todo));
    }
    _logger.LogDebug($"Запрошенный todo с id: {id} не найден");
    return NotFound();
  }

  // POST: api/todos
  [HttpPost]
  public async Task<ActionResult<TodoReadDTO>> CreateTodoAsync(TodoCreateDto todoCreateDto)
  {
    if (todoCreateDto is null)
    {
      throw new Exception("Неверные данные");
    }
    //Здесь модель в смысле модель для базы данных
    var todoModel = _mapper.Map<Todo>(todoCreateDto);
    _todoValidator.Validate(todoModel).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }
    await _todoRepo.CreateTodoAuthAsync(todoModel);
    await _todoRepo.SaveChangesAsync();
    var todoReadDto = _mapper.Map<TodoReadDTO>(todoModel);
    return CreatedAtAction(nameof(GetTodoByIdAsync), new { id = todoReadDto.Guid }, todoReadDto);
  }

  // PUT: api/todos/{id}
  [HttpPut("{id:guid}")]
  public async Task<ActionResult> UpdateTodoAsync(Guid id, TodoUpdateDto todoUpdateDto)
  {
    if (todoUpdateDto is null)
    {
      throw new Exception("Неверные данные");
    }
    var todoModelFromRepo = await _todoRepo.GetTodoByGuidAuthAsync(id);
    if (todoModelFromRepo is null)
    {
      return NotFound();
    }
    _mapper.Map(todoUpdateDto, todoModelFromRepo);
    _todoValidator.Validate(todoModelFromRepo).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }
    _todoRepo.UpdateTodoAuth(todoModelFromRepo);
    await _todoRepo.SaveChangesAsync();
    return NoContent();
  }

  // PATCH: api/todos/{id}
  [HttpPatch("{id:guid}")]
  public async Task<ActionResult> PartialTodoUpdateAsync(Guid id, JsonPatchDocument<TodoUpdateDto> patchDoc)
  {
    var todoModelFromRepo = await _todoRepo.GetTodoByGuidAuthAsync(id);
    if (todoModelFromRepo is null)
    {
      return NotFound();
    }
    var todoToPatch = _mapper.Map<TodoUpdateDto>(todoModelFromRepo);
    patchDoc.ApplyTo(todoToPatch, ModelState);
    _mapper.Map(todoToPatch, todoModelFromRepo);
    _todoValidator.Validate(todoModelFromRepo, null).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }
    _todoRepo.UpdateTodoAuth(todoModelFromRepo);
    await _todoRepo.SaveChangesAsync();
    return NoContent();
  }

  // DELETE: api/todos/{id}
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteTodoAsync(Guid id)
  {
    var todoModelFromRepo = await _todoRepo.GetTodoByGuidAuthAsync(id);
    if (todoModelFromRepo is null)
    {
      return NotFound();
    }
    await _todoRepo.DeleteTodoByGuidAuthAsync(todoModelFromRepo.Guid);
    await _todoRepo.SaveChangesAsync();
    return NoContent();
  }
}
