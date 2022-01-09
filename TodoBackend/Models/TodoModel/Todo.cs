#nullable disable
using System;

namespace TodoApp.Backend.Models.TodoModel;

public class Todo : ITimeStamped
{
  public Guid Guid { get; set; }
  public string Title { get; set; }
  public string Body { get; set; }
  public Guid UserGuid { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
