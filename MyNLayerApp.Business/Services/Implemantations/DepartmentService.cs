using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Business.Services.Interfaces;
using MyNLayerApp.Business.Dtos.DepartmentDtos;

namespace MyNLayerApp.Business.Services.Implemantations;

public class DepartmentService : IDepartmentService
{
    private readonly MyNLayerAppDbContext _context;
    public DepartmentService(MyNLayerAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<DepartmentReturnDto>> GetAllDepartmentsAsync()
    {
        return await _context.Departments.AsNoTracking()
            .Select(d => new DepartmentReturnDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Location = d.Location,
                Limit = d.Limit,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate
            })
            .ToListAsync();
    }
    public async Task<DepartmentReturnDto?> GetDepartmentByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid department ID.");
        }
        var department = await _context.Departments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with ID {id} not found.");
        }
        var departmentReturnDto = new DepartmentReturnDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Limit = department.Limit,
            CreatedDate = department.CreatedDate,
            UpdatedDate = department.UpdatedDate,
            Employees = await _context.Employees
                .Where(e => e.DepartmentId == department.Id)
                .Select(e => new EmployeeInDepartmentReturnDto
                {
                    Name = e.Name,
                    
                })
                .ToListAsync()
        };
        return departmentReturnDto;

    }
    public async Task<Department> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto)
    {
        if (departmentCreateDto == null)
        {
            throw new ArgumentNullException(nameof(departmentCreateDto), "Department create DTO cannot be null.");
        }
        var department = new Department
        {
            Name = departmentCreateDto.Name,
            Description = departmentCreateDto.Description,
            Location = departmentCreateDto.Location,
            Limit = departmentCreateDto.Limit
        };
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department), "Department cannot be null.");
        }
        var existingDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.Name == department.Name);
        if (existingDepartment != null)
        {
            throw new InvalidOperationException("A department with the same name already exists.");
        }
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }
    public async Task<Department> UpdateDepartmentAsync(Department department)
    {
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department), "Department cannot be null.");
        }
        var existingDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == department.Id);
        if (existingDepartment == null)
        {
            throw new KeyNotFoundException($"Department with ID {department.Id} not found.");
        }
        var duplicateDepartment = await _context.Departments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name == department.Name && d.Id != department.Id);
        if (duplicateDepartment != null)
        {
            throw new InvalidOperationException("A department with the same name already exists.");
        }
        existingDepartment.Name = department.Name;
        existingDepartment.Description = department.Description;
        existingDepartment.UpdatedDate = DateTime.UtcNow;
        _context.Departments.Update(existingDepartment);
        await _context.SaveChangesAsync();
        return existingDepartment;
    }
    public async Task DeleteDepartmentAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid department ID.");
        }
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with ID {id} not found.");
        }
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }
}
