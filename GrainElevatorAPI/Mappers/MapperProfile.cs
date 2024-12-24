using AutoMapper;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;

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
        CreateMap<Employee, EmployeeUpdateRequest>().ReverseMap();
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleCreateRequest>().ReverseMap();;
        
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<Supplier, SupplierCreateRequest>().ReverseMap();
        
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductCreateRequest>().ReverseMap();
        
        CreateMap<InputInvoice, InputInvoiceCreateRequest>().ReverseMap();
        CreateMap<InputInvoice, InputInvoiceUpdateRequest>().ReverseMap();
        CreateMap<InputInvoice, InputInvoiceDto>()
            .ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))
            .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.LastName))
            .ReverseMap();
        
        CreateMap<LaboratoryCard, LaboratoryCardCreateRequest>().ReverseMap();
        CreateMap<LaboratoryCard, LaboratoryCardUpdateRequest>().ReverseMap();
        CreateMap<LaboratoryCard, LaboratoryCardDto>()
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InputInvoice.InvoiceNumber))
            .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.InputInvoice.ArrivalDate))
            .ForMember(dest => dest.PhysicalWeight, opt => opt.MapFrom(src => src.InputInvoice.PhysicalWeight))
            .ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.InputInvoice.Supplier.Title))
            .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.InputInvoice.Product.Title))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.LastName));
        
        CreateMap<ProductionBatch, ProductionBatchDto>().ReverseMap();
        
        CreateMap<InvoiceRegister, InvoiceRegisterCreateRequest>().ReverseMap();
        CreateMap<InvoiceRegister, InvoiceRegisterUpdateRequest>().ReverseMap();
        CreateMap<InvoiceRegister, InvoiceRegisterDto>()
            .ForMember(dest => dest.SupplierTitle, opt => opt.MapFrom(src => src.Supplier.Title))
            .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.LastName))
            .ReverseMap();
        
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryCreateRequest>().ReverseMap();
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryUpdateRequest>().ReverseMap();
        CreateMap<WarehouseProductCategory, WarehouseProductCategoryDto>().ReverseMap();
        
        CreateMap<WarehouseUnit, WarehouseUnitCreateRequest>().ReverseMap();
        CreateMap<WarehouseUnit, WarehouseUnitUpdateRequest>().ReverseMap();
        CreateMap<WarehouseUnit, WarehouseUnitDto>().ReverseMap();
        
        CreateMap<OutputInvoice, OutputInvoiceCreateRequest>().ReverseMap();
        CreateMap<OutputInvoice, OutputInvoiceUpdateRequest>().ReverseMap();
        CreateMap<OutputInvoice, OutputInvoiceDto>().ReverseMap();
        
        CreateMap<CompletionReport, CompletionReportCreateRequest>().ReverseMap();
        CreateMap<CompletionReport, CompletionReportUpdateRequest>().ReverseMap();
        CreateMap<CompletionReport, CompletionReportDto>().ReverseMap();
        
        CreateMap<CompletionReportOperation, CompletionReportOperationCreateRequest>().ReverseMap();
        CreateMap<CompletionReportOperation, CompletionReportOperationUpdateRequest>().ReverseMap();
        CreateMap<CompletionReportOperation, CompletionReportOperationDto>().ReverseMap();
        
        CreateMap<PriceList, PriceListCreateRequest>().ReverseMap();
        CreateMap<PriceList, PriceListUpdateRequest>().ReverseMap();
        CreateMap<PriceList, PriceListDto>().ReverseMap();
        
        CreateMap<PriceListItem, PriceListItemCreateRequest>().ReverseMap();
        CreateMap<PriceListItem, PriceListItemUpdateRequest>().ReverseMap();
        CreateMap<PriceListItem, PriceListItemDto>().ReverseMap();
        
        CreateMap<TechnologicalOperation, TechnologicalOperationCreateRequest>().ReverseMap();
        CreateMap<TechnologicalOperation, TechnologicalOperationDto>().ReverseMap();
        
    }
}