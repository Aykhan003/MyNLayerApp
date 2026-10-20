using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.DataAccess.Configurations;

public class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
{
    public void Configure(EntityTypeBuilder<EmployeeProject> builder)
    {
        builder.ToTable("EmployeeProjects");
        builder.HasKey(x => new { x.EmployeeId, x.ProjectId });
        builder.HasOne(x => x.Employee)
            .WithMany(e => e.EmployeeProjects)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Project)
            .WithMany(p => p.EmployeeProjects)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
