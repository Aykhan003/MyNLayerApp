using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.DataAccess.Contexts;

public class MyNLayerAppDbContext : DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=MyNLayerAppDb;Trusted_Connection=True;TrustServerCertificate=true;");
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<Department> Departments { get; set; }
}
