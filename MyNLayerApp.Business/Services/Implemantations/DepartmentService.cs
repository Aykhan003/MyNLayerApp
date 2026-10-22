using MyNLayerApp.DataAccess.Contexts;
using MyNLayerApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using MyNLayerApp.Business.Services.Interfaces;
using MyNLayerApp.Business.Dtos.DepartmentDtos;
using AutoMapper;

namespace MyNLayerApp.Business.Services.Implemantations;

public class DepartmentService : IDepartmentService
{
    private readonly MyNLayerAppDbContext _context;
    private readonly IMapper _mapper;
    public DepartmentService(MyNLayerAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<DepartmentReturnDto>> GetAllDepartmentsAsync()
    {
        return await _context.Departments.AsNoTracking()
            .Select(d => _mapper.Map<DepartmentReturnDto>(d))
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
        var departmentReturnDto = _mapper.Map<DepartmentReturnDto>(department);
        return departmentReturnDto;

    }
    public async Task<Department> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto)
    {
        if (departmentCreateDto == null)
        {
            throw new ArgumentNullException(nameof(departmentCreateDto), "Department create DTO cannot be null.");
        }
        var department = _mapper.Map<Department>(departmentCreateDto);
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
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
        return department;
    }
    public async Task<Department> UpdateDepartmentAsync(Department department)
    {
        var departmentReturnDto = await GetDepartmentByIdAsync(department.Id);
        var duplicateDepartment = await _context.Departments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name == department.Name && d.Id != department.Id);
        if (duplicateDepartment != null)
        {
            throw new InvalidOperationException("A department with the same name already exists.");
        }
        var existingDepartment = _mapper.Map<Department>(departmentReturnDto);
        _context.Departments.Update(existingDepartment);
        await _context.SaveChangesAsync();
        return existingDepartment;
    }
    public async Task DeleteDepartmentAsync(int id)
    {
        var departmentReturnDto = await GetDepartmentByIdAsync(id);
        var department = _mapper.Map<Department>(departmentReturnDto);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with ID {id} not found.");
        }
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }
}
