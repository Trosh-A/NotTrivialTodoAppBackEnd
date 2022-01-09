using FluentValidation;
using Microsoft.Extensions.Options;
using TodoApp.Backend.Constants;
using TodoApp.Backend.Models.TodoModel;

namespace TodoApp.Backend.Models.Validators;

public class TodoValidator : AbstractValidator<Todo>
{
  public TodoValidator()
  {
    RuleFor(x => x.Title).NotEmpty().MaximumLength(TodoConstants.TodoTitleMaxLength);
    RuleFor(x => x.Body).NotEmpty().MaximumLength(TodoConstants.TodoBodyMaxLength);
  }
}
