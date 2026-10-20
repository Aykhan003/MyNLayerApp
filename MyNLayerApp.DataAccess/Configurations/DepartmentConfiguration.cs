using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.DataAccess.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.Location).HasMaxLength(100);
        builder.Property(x => x.Limit).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now);
    }
}
