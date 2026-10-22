using MyNLayerApp.Business.Dtos.EmployeeDtos;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeCreateDto> CreateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<List<EmployeeReturnDto>> GetAllEmployeesAsync();
        Task<EmployeeReturnDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeCreateDto> UpdateEmployeeAsync(Employee employee);
    }
}