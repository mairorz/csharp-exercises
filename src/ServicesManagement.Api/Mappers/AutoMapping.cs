using AutoMapper;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<AddUserDto, User>();
    }
}