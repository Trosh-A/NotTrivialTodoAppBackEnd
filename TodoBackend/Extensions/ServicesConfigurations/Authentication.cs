using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoApp.Backend.Extensions.ServicesConfigurations;

public static class Authentication
{
  public static IServiceCollection ConfigureAuthentication(this IServiceCollection isc, string jwtSecretKey)
  {
    isc.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
    {
      x.RequireHttpsMetadata = false;
      x.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuerSigningKey = true,
      };
    });
    return isc;
  }
}
