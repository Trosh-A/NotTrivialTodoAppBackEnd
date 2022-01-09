#nullable disable
using System;
using System.Collections.Generic;

namespace TodoApp.Backend.Models.UserModel;

public class UserReadDto
{
  public Guid Guid { get; set; }
  public string UserName { get; set; }
  public IEnumerable<string> Roles { get; set; }
}
