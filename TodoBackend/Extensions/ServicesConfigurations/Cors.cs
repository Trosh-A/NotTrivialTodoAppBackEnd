using Microsoft.Extensions.DependencyInjection;
using TodoApp.Backend.Constants;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class Cors
{
  public static IServiceCollection ConfigureCors(this IServiceCollection services)
  {
    return services.AddCors(options =>
    {
      options.AddPolicy(CorsConstants.CorsAnyPolicy,
              builder => builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
    });
  }
}
