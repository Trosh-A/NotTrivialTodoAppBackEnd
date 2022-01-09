using AutoMapper;
using TodoApp.Backend.Models.TodoModel;

namespace TodoApp.Backend.Models.AutoMapperProfiles;

public class TodoProfile : Profile
{
  public TodoProfile()
  {
    CreateMap<Todo, TodoReadDTO>();
    CreateMap<TodoCreateDto, Todo>();
    CreateMap<TodoUpdateDto, Todo>();
    CreateMap<Todo, TodoUpdateDto>();
  }
}
