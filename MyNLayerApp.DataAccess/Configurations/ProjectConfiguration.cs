using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.DataAccess.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Projects>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Projects> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired(false);
        builder.Property(x => x.Budget).IsRequired().HasColumnType("decimal(18,2)");

        builder.HasMany(x => x.EmployeeProjects)
            .WithOne(ep => ep.Project)
            .HasForeignKey(ep => ep.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
