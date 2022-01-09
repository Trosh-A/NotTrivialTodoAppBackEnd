using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Backend.EFCore;
using TodoApp.Backend.Models.TodoModel;
using TodoApp.Backend.Repositories.Interfaces;

namespace TodoApp.Backend.Repositories;

public class TodosRepo : ITodosRepo
{
  private readonly AppDbContext _ctx;
  private readonly Guid? userGuid = null;

  public TodosRepo(AppDbContext ctx, IHttpContextAccessor httpContextAccessor)
  {
    _ctx = ctx;
    var userGuidString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (Guid.TryParse(userGuidString, out Guid userGuidTemp)) userGuid = userGuidTemp;
  }
  public async Task<IEnumerable<Todo>> GetAllTodosAuthAsync()
  {
    return await _ctx.Todos.AsNoTracking().Where(t => t.Guid == userGuid).ToListAsync();
  }
  public async Task<Todo?> GetTodoByGuidAuthAsync(Guid guid)
  {
    return await _ctx.Todos.SingleOrDefaultAsync(t => t.Guid == guid && t.UserGuid == userGuid);
  }
  public async Task<Todo?> CreateTodoAuthAsync(Todo todo)
  {
    if (todo is null || userGuid is null) return null;
    todo.UserGuid = (Guid)userGuid;
    _ctx.Todos.Add(todo);
    await _ctx.SaveChangesAsync();
    return todo;
  }
  public Todo? UpdateTodoAuth(Todo todo)
  {
    if (todo is null || userGuid is null) return null;
    return todo;
  }
  public async Task<Todo?> DeleteTodoByGuidAuthAsync(Guid guid)
  {
    if (userGuid is null) return null;
    var todo = await _ctx.Todos.SingleOrDefaultAsync(t => t.UserGuid == userGuid && t.Guid == guid);
    if (todo is null) return null;
    return _ctx.Todos.Remove(todo).Entity;
  }
  public async Task<bool> TodoExistsAsync(Guid guid)
  {
    if (userGuid is null) return false;
    return await _ctx.Todos.Where(t => t.Guid == guid && t.UserGuid == userGuid).AnyAsync();
  }
  public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

  public async Task<int> GetAllTodosCountAsync()
  {
    return await _ctx.Todos.CountAsync();
  }
}
