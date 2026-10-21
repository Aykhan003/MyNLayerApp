using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Services.Interfaces
{
    public interface IEmployeeProjectService
    {
        Task<EmployeeProject> CreateEmployeeProjectAsync(EmployeeProject employeeProject);
        Task DeleteEmployeeProjectAsync(int employeeId, int projectId);
        Task<List<EmployeeProject>> GetAllEmployeeProjectsAsync();
        Task<EmployeeProject?> GetEmployeeProjectByIdAsync(int employeeId, int projectId);
        Task<EmployeeProject> UpdateEmployeeProjectAsync(EmployeeProject employeeProject);
    }
}