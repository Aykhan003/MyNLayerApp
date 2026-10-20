using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Entity.Entities;
using MyNLayerApp.Entity.Entities.Enums;

namespace MyNLayerApp.DataAccess.Configurations;

public class EmployeeConfiguration
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Surname).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.Property(x => x.HireDate).IsRequired();
        builder.Property(x => x.Salary).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Status).IsRequired().HasDefaultValue(EmployeeStatus.Active);
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now);

        builder.HasOne(x => x.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
