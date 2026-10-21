using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Projects> CreateProjectAsync(Projects project);
        Task DeleteProjectAsync(int id);
        Task<List<Projects>> GetAllProjectsAsync();
        Task<Projects?> GetProjectByIdAsync(int id);
        Task<Projects> UpdateProjectAsync(Projects project);
    }
}