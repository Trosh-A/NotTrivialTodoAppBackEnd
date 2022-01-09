using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using TodoApp.Backend.Constants;
using TodoApp.Backend.EFCore;
using TodoApp.Backend.Extensions;
using TodoApp.Backend.Extensions.ServicesConfigurations;
using TodoApp.Backend.Models;
using TodoApp.Backend.Models.AutoMapperProfiles;
using TodoApp.Backend.Repositories;
using TodoApp.Backend.Repositories.Interfaces;
using TodoApp.Backend.Services.JwtGenerator;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Configuration.AddJsonsFromDirectory("Configurations");
  builder.Logging.ClearProviders();
  builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
  builder.Host.UseNLog();
  builder.Services
      .ConfigureControllers()
      .ConfigureNewtonsoftJson()
      .ConfigureFluentValidation();
  builder.Services.ConfigureAuthentication(builder.Configuration["JwtSecretKey"]);
  builder.Services.ConfigureIdentity();
  builder.Services.AddAutoMapper(typeof(TodoProfile));
  builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
  builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
  builder.Services.ConfigureCors();
  builder.Services.AddHttpContextAccessor();

  builder.Services.AddScoped<IUsersRepo, UsersRepo>();
  builder.Services.AddScoped<ITodosRepo, TodosRepo>();
  builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  var app = builder.Build();
  await app.AddSeedDataAsync(logger);
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }
  app.UseHttpsRedirection();
  app.UseStaticFiles();
  app.UseCors(CorsConstants.CorsAnyPolicy);
  app.UseAuthentication();
  app.UseAuthorization();
  app.MapControllers();
  app.Run();
}
catch (Exception ex)
{
  logger.Error(ex, "Stopped program because of exception");
  throw;
}
finally
{
  NLog.LogManager.Shutdown();
}
