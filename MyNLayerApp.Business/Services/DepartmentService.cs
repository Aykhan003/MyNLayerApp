using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyNLayerApp.Business.Services;

public class DepartmentService
{
    private readonly MyNLayerAppDbContext _context;
    public DepartmentService(MyNLayerAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _context.Departments.AsNoTracking()
            .ToListAsync();
    }
    public async Task<Department?> GetDepartmentByIdAsync(int id)
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
        return await _context.Departments.FindAsync(id);
    }
    public async Task<Department> CreateDepartmentAsync(Department department)
    {
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
        var existingDepartment = await _context.Departments.AsNoTracking()
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
