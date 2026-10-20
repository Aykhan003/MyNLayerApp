using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.DataAccess.Contexts;

public class MyNLayerAppDbContext : DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=MyNLayerAppDb2;Trusted_Connection=True;TrustServerCertificate=true;");
        base.OnConfiguring(optionsBuilder);
    }
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyNLayerAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Projects> Projects { get; set; }
    public DbSet<EmployeeProject> EmployeeProjects { get; set; }
}

