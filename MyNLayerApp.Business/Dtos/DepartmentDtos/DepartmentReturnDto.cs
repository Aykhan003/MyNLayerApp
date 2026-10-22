using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Dtos.DepartmentDtos;

public class DepartmentReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Limit { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<EmployeeInDepartmentReturnDto> Employees { get; set; }
}

public class EmployeeInDepartmentReturnDto
{
    public string Name { get; set; } = null!;
    public string? Surname { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
}
