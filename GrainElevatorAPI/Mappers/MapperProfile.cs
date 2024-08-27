using AutoMapper;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Employee, EmployeeDTO>()
            // .ForMember(dest => dest.Nickname,
            //     opt => opt.MapFrom(
            //         src => src.UserName))  -  якщо назви полів моделі і ДТО не співпадають
            .ReverseMap();

        CreateMap<Employee, EmployeeRegisterRequest>().ReverseMap();
        CreateMap<Role, RoleDTO>().ReverseMap();;
        CreateMap<Supplier, SupplierDTO>().ReverseMap();
        CreateMap<ProductTitle, ProductTitleDTO>().ReverseMap();;
    }
}