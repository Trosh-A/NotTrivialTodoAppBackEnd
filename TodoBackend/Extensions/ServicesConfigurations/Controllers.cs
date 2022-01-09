using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class Controllers
{
  public static IMvcBuilder ConfigureControllers(this IServiceCollection services)
  {
    return services.AddControllers(x => { x.SuppressAsyncSuffixInActionNames = false; });
  }
}
