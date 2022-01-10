using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class NewtonsoftJson
{
  public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder mvcBuilder)
  {
    return mvcBuilder.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
  }
}
