namespace api_pim.Mappers;

using AutoMapper;
using api_pim.Models;
using api_pim.Entities;

public class DescontoMappingProfile : Profile
{
    public DescontoMappingProfile()
    {
        CreateMap<CreateDescontoRequest, Desconto>();
    }
}