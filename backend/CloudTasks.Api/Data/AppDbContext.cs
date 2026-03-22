using CloudTasks.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTasks.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskEntity>(e =>
        {
            e.ToTable("Tasks");
            e.Property(t => t.Name).IsRequired();
        });
    }
}
