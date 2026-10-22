using AutoMapper;
using MyNLayerApp.Business.Dtos.DepartmentDtos;
using MyNLayerApp.Business.Dtos.EmployeeDtos;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Department, DepartmentReturnDto>().ReverseMap();
        CreateMap<Employee, EmployeeInDepartmentReturnDto>().ReverseMap();
        CreateMap<DepartmentCreateDto, Department>();

        CreateMap<Employee, EmployeeReturnDto>().ReverseMap();
        CreateMap<EmployeeCreateDto, Employee>();
        CreateMap<Projects, ProjectInEmployeeReturnDto>();

    }
}
