using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;
using MyNLayerApp.Entity.Entities.Common;

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
    public override int SaveChanges()
    {
        var datas = ChangeTracker.Entries<AuditEntity>().ToList();
        foreach (var data in datas)
        {
            switch (data.State)
            {
                case EntityState.Added:
                    data.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    data.Entity.UpdatedDate = DateTime.Now;
                    break;
            }
        }
        return base.SaveChanges();
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Projects> Projects { get; set; }
    public DbSet<EmployeeProject> EmployeeProjects { get; set; }
}

