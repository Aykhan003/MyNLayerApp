namespace MyNLayerApp.Business.Dtos.EmployeeDtos;

public class EmployeeCreateDto
{
    public string Name { get; set; } = null!;
    public string? Surname { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public int Status { get; set; }
    public int DepartmentId { get; set; }
}
