using System;

namespace TodoApp.Backend.Models
{
  public interface ITimeStamped
  {
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
