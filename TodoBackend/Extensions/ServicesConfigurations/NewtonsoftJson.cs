using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Backend.Models.Validators;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class NewtonsoftJson
{
  public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder mvcBuilder)
  {
    return mvcBuilder.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
  }
}
