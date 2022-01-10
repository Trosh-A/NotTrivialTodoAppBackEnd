using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Backend.Constants;
using TodoApp.Backend.Models.TodoModel;

namespace TodoApp.Backend.EFCore.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
  public void Configure(EntityTypeBuilder<Todo> builder)
  {
    builder.HasKey(t => t.Guid);
    builder.Property(t => t.Title).HasMaxLength(TodoConstants.TodoTitleMaxLength).IsRequired();
    builder.Property(t => t.Body).HasMaxLength(TodoConstants.TodoBodyMaxLength).IsRequired();
  }
}
