using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Backend.Models.TodoModel;

namespace TodoApp.Backend.Repositories.Interfaces;

public interface ITodosRepo
{
  Task<int> SaveChangesAsync();
  Task<IEnumerable<Todo>> GetAllTodosAuthAsync();
  Task<Todo?> GetTodoByGuidAuthAsync(Guid guid);
  Task<Todo?> CreateTodoAuthAsync(Todo todo);
  Todo? UpdateTodoAuth(Todo todo);
  Task<Todo?> DeleteTodoByGuidAuthAsync(Guid guid);
  Task<bool> TodoExistsAsync(Guid guid);
  Task<int> GetAllTodosCountAsync();
}
