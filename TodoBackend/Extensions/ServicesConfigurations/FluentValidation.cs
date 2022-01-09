using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Backend.Models.Validators;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class FluentValidation
{
  public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder mvcBuilder)
  {
    return mvcBuilder.AddFluentValidation(x =>
     {
       x.DisableDataAnnotationsValidation = true;
       x.RegisterValidatorsFromAssemblyContaining<TodoValidator>();
     });
  }
}
