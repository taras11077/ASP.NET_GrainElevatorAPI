using AutoMapper;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            // .ForMember(dest => dest.Nickname,
            //     opt => opt.MapFrom(
            //         src => src.UserName))  -  якщо назви полів моделі і ДТО не співпадають
            .ReverseMap();

        CreateMap<Employee, EmployeeRegisterRequest>().ReverseMap();
        
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleCreateRequest>().ReverseMap();;
        
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<Supplier, SupplierCreateRequest>().ReverseMap();
        
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductCreateRequest>().ReverseMap();
        
        CreateMap<InputInvoice, InputInvoiceCreateRequest>().ReverseMap();
        CreateMap<InputInvoice, InputInvoiceUpdateRequest>().ReverseMap();
        CreateMap<InputInvoice, InputInvoiceDto>().ReverseMap();
        
        CreateMap<LaboratoryCard, LaboratoryCardCreateRequest>().ReverseMap();
        CreateMap<LaboratoryCard, LaboratoryCardUpdateRequest>().ReverseMap();
        CreateMap<LaboratoryCard, LaboratoryCardDto>().ReverseMap();
        
        CreateMap<ProductionBatch, ProductionBatchDto>().ReverseMap();
        
        CreateMap<InvoiceRegister, InvoiceRegisterCreateRequest>().ReverseMap();
        CreateMap<InvoiceRegister, InvoiceRegisterUpdateRequest>().ReverseMap();
        CreateMap<InvoiceRegister, InvoiceRegisterDto>().ReverseMap();
        
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryCreateRequest>().ReverseMap();
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryUpdateRequest>().ReverseMap();
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryDto>().ReverseMap();
    }
}