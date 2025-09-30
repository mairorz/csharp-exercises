using AutoMapper;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        //User
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<User, GetUserDto>();
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        //Client
        CreateMap<AddClientDto, Client>();
        CreateMap<Client, GetClientDto>();
        CreateMap<UpdateClientDto, Client>();

        //Service
        CreateMap<AddServiceDto, Service>();
        CreateMap<Service, GetServiceDto>();
        CreateMap<UpdateServiceDto, Service>();

        //Invoice
        CreateMap<AddInvoiceDto, Invoice>();
        CreateMap<Invoice, GetInvoiceDto>();
        CreateMap<UpdateInvoiceDto, Invoice>();

        //Invoice_Detail
        CreateMap<AddInvoiceDetailDto, InvoiceDetail>();
        CreateMap<InvoiceDetail, GetInvoiceDetailDto>();
        CreateMap<UpdateInvoiceDetailDto, InvoiceDetail>();

        //Supplier
        CreateMap<AddSupplierDto, Supplier>();
        CreateMap<Supplier, GetSupplierDto>();
        CreateMap<UpdateSupplierDto, Supplier>();

        //Inputs_Categories
        CreateMap<AddInputsCategoriesDto, InputsCategories>();
        CreateMap<InputsCategories, GetInputsCategoriesDto>();
        CreateMap<UpdateInputsCategoriesDto, InputsCategories>();

        //Inputs
        CreateMap<AddInputsDto, Inputs>();
        CreateMap<Inputs, GetInputsDto>();
        CreateMap<UpdateInputsDto, Inputs>();

        //Purchases
        CreateMap<AddPurchasesDto, Purchases>();
        CreateMap<Purchases, GetPurchasesDto>();
        CreateMap<UpdatePurchasesDto, Purchases>();

        //Work_Order
        CreateMap<AddWorkOrderDto, WorkOrder>();
        CreateMap<WorkOrder, GetWorkOrderDto>();
        CreateMap<UpdateWorkOrderDto, WorkOrder>();

        //Consumption
        CreateMap<AddConsumptionDto, Consumption>();
        CreateMap<Consumption, GetConsumptionDto>();
        CreateMap<UpdateConsumptionDto, Consumption>();
    }
}