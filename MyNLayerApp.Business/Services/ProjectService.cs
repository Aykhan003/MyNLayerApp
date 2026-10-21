using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyNLayerApp.Business.Services;

public class ProjectService
{
    private readonly MyNLayerAppDbContext _context;
    public ProjectService(MyNLayerAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Projects>> GetAllProjectsAsync()
    {
        return await _context.Projects.AsNoTracking()
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .ToListAsync();
    }
    public async Task<Projects?> GetProjectByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid project ID.");
        }
        var project = await _context.Projects.AsNoTracking()
            .Include(p => p.EmployeeProjects)
            .ThenInclude(ep => ep.Employee)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {id} not found.");
        }
        if (project.EmployeeProjects == null || !project.EmployeeProjects.Any())
        {
            throw new InvalidOperationException($"Project with ID {id} has no associated employees.");
        }
        return project;
    }
    public async Task<Projects> CreateProjectAsync(Projects project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");
        }
        var existingProject = await _context.Projects
            .FirstOrDefaultAsync(p => p.Name == project.Name);
        if (existingProject != null)
        {
            throw new InvalidOperationException("A project with the same name already exists.");
        }
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }
    public async Task<Projects> UpdateProjectAsync(Projects project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");
        }
        var existingProject = await _context.Projects.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == project.Id);
        if (existingProject == null)
        {
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");
        }
        var duplicateProject = await _context.Projects.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == project.Name && p.Id != project.Id);
        if (duplicateProject != null)
        {
            throw new InvalidOperationException("A project with the same name already exists.");
        }
        existingProject.Name = project.Name;
        existingProject.Description = project.Description;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.Budget = project.Budget;
        existingProject.UpdatedDate = DateTime.UtcNow;
        _context.Projects.Update(existingProject);
        await _context.SaveChangesAsync();
        return existingProject;
    }
    public async Task DeleteProjectAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid project ID.");
        }
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {id} not found.");
        }
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}
