using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyNLayerApp.Business.Services;

public class EmployeeService
{
    private readonly MyNLayerAppDbContext _context;
    public EmployeeService(MyNLayerAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.AsNoTracking()
            .Include(e => e.Department)
            .ToListAsync();
    }
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid employee ID.");
        }
        var employee = await _context.Employees.AsNoTracking().
            Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }
        return employee;
    }
    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
        }
        var department = await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with ID {employee.DepartmentId} not found.");
        }
        if (department.Employees.Count >= department.Limit)
        {
            throw new InvalidOperationException($"Cannot add employee. Department '{department.Name}' has reached its limit of {department.Limit} employees.");
        }
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == employee.Email);
        if (existingEmployee != null)
        {
            throw new InvalidOperationException("An employee with the same email already exists.");
        }

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
        }
        var existingEmployee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == employee.Id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employee.Id} not found.");
        }
        if (existingEmployee.DepartmentId != employee.DepartmentId)
        {
            var newDepartment = await _context.Departments.AsNoTracking().
                Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);
            if (newDepartment == null)
            {
                throw new KeyNotFoundException($"Department with ID {employee.DepartmentId} not found.");
            }
            if (newDepartment.Employees.Count >= newDepartment.Limit)
            {
                throw new InvalidOperationException($"Cannot move employee to department '{newDepartment.Name}'. It has reached its limit of {newDepartment.Limit} employees.");
            }
        }
        var duplicateEmployee = await _context.Employees.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Email == employee.Email && e.Id != employee.Id);
        if (duplicateEmployee != null)
        {
            throw new InvalidOperationException("An employee with the same email already exists.");
        }
        existingEmployee.Name = employee.Name;
        existingEmployee.Surname = employee.Surname; 
        existingEmployee.PhoneNumber = employee.PhoneNumber;
        existingEmployee.HireDate = employee.HireDate;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.Status = employee.Status;
        existingEmployee.Email = employee.Email;
        existingEmployee.DepartmentId = employee.DepartmentId;
        existingEmployee.UpdatedDate = DateTime.UtcNow;
        _context.Employees.Update(existingEmployee);
        await _context.SaveChangesAsync();
        return existingEmployee;
    }
    public async Task DeleteEmployeeAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid employee ID.");
        }
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}
