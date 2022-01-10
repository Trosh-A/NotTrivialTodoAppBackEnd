#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Backend.EFCore.Configurations;
using TodoApp.Backend.Models;
using TodoApp.Backend.Models.TodoModel;

namespace TodoApp.Backend.EFCore;

public class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
  public DbSet<Todo> Todos { get; set; }
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
    Database.EnsureCreated();
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoConfiguration).Assembly);
  }

  public override int SaveChanges()
  {
    AddTimeStamps();
    return base.SaveChanges();
  }
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    AddTimeStamps();
    return await base.SaveChangesAsync(cancellationToken);
  }
  private void AddTimeStamps()
  {
    var newEntities = ChangeTracker.Entries()
        .Where(
            x => x.State == EntityState.Added &&
            x.Entity != null &&
            x.Entity as ITimeStamped != null
            )
        .Select(x => x.Entity as ITimeStamped);

    var modifiedEntities = ChangeTracker.Entries()
        .Where(
            x => x.State == EntityState.Modified &&
            x.Entity != null &&
            x.Entity as ITimeStamped != null
            )
        .Select(x => x.Entity as ITimeStamped);

    foreach (var newEntity in newEntities)
    {
      newEntity.CreatedAt = DateTime.UtcNow;
      newEntity.UpdatedAt = DateTime.UtcNow;
    }

    foreach (var modifiedEntity in modifiedEntities)
    {
      modifiedEntity.UpdatedAt = DateTime.UtcNow;
    }
  }

}
