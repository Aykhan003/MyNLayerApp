using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyNLayerApp.Business.Services;

public class EmployeeProjectService
{
    private readonly MyNLayerAppDbContext _context;
    public EmployeeProjectService(MyNLayerAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<EmployeeProject>> GetAllEmployeeProjectsAsync()
    {
        return await _context.EmployeeProjects.AsNoTracking()
            .Include(ep => ep.Employee)
            .Include(ep => ep.Project)
            .ToListAsync();
    }
    public async Task<EmployeeProject?> GetEmployeeProjectByIdAsync(int employeeId, int projectId)
    {
        if (employeeId <= 0 || projectId <= 0)
        {
            throw new ArgumentException("Invalid employee or project ID.");
        }
        var employeeProject = await _context.EmployeeProjects.AsNoTracking()
            .Include(ep => ep.Employee)
            .Include(ep => ep.Project)
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);
        if (employeeProject == null)
        {
            throw new KeyNotFoundException($"EmployeeProject with Employee ID {employeeId} and Project ID {projectId} not found.");
        }
        return employeeProject;
    }
    public async Task<EmployeeProject> CreateEmployeeProjectAsync(EmployeeProject employeeProject)
    {
        if (employeeProject == null)
        {
            throw new ArgumentNullException(nameof(employeeProject), "EmployeeProject cannot be null.");
        }
        var existingEmployee = await _context.Employees.FindAsync(employeeProject.EmployeeId);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employeeProject.EmployeeId} not found.");
        }
        var existingProject = await _context.Projects.FindAsync(employeeProject.ProjectId);
        if (existingProject == null)
        {
            throw new KeyNotFoundException($"Project with ID {employeeProject.ProjectId} not found.");
        }
        var existingEmployeeProject = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeProject.EmployeeId && ep.ProjectId == employeeProject.ProjectId);
        if (existingEmployeeProject != null)
        {
            throw new InvalidOperationException("This employee is already assigned to the project.");
        }
        _context.EmployeeProjects.Add(employeeProject);
        await _context.SaveChangesAsync();
        return employeeProject;
    }
    public async Task<EmployeeProject> UpdateEmployeeProjectAsync(EmployeeProject employeeProject)
    {
        if (employeeProject == null)
        {
            throw new ArgumentNullException(nameof(employeeProject), "EmployeeProject cannot be null.");
        }
        var existingEmployeeProject = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeProject.EmployeeId && ep.ProjectId == employeeProject.ProjectId);
        if (existingEmployeeProject == null)
        {
            throw new KeyNotFoundException($"EmployeeProject with Employee ID {employeeProject.EmployeeId} and Project ID {employeeProject.ProjectId} not found.");
        }
        if (existingEmployeeProject.EmployeeId != employeeProject.EmployeeId || existingEmployeeProject.ProjectId != employeeProject.ProjectId)
        {
            var duplicateEmployeeProject = await _context.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeProject.EmployeeId && ep.ProjectId == employeeProject.ProjectId);
            if (duplicateEmployeeProject != null)
            {
                throw new InvalidOperationException("This employee is already assigned to the project.");
            }
        }
        existingEmployeeProject.EmployeeId = employeeProject.EmployeeId;
        existingEmployeeProject.ProjectId = employeeProject.ProjectId;
        _context.EmployeeProjects.Update(existingEmployeeProject);
        await _context.SaveChangesAsync();
        return existingEmployeeProject;
    }
    public async Task DeleteEmployeeProjectAsync(int employeeId, int projectId)
    {
        if (employeeId <= 0 || projectId <= 0)
        {
            throw new ArgumentException("Invalid employee or project ID.");
        }
        var existingEmployeeProject = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);
        if (existingEmployeeProject == null)
        {
            throw new KeyNotFoundException($"EmployeeProject with Employee ID {employeeId} and Project ID {projectId} not found.");
        }
        _context.EmployeeProjects.Remove(existingEmployeeProject);
        await _context.SaveChangesAsync();
    }
}
