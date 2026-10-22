using AutoMapper;
using MyNLayerApp.Business.Dtos.DepartmentDtos;
using MyNLayerApp.Entity.Entities;

namespace MyNLayerApp.Business.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Department, DepartmentReturnDto>().ReverseMap();
        CreateMap<Employee, EmployeeInDepartmentReturnDto>().ReverseMap();
        CreateMap<DepartmentCreateDto, Department>();
    }
}
