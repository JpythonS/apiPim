namespace api_pim.Mappers;

using AutoMapper;
using api_pim.Models;
using api_pim.Entities;

public class AdicionalMappingProfile : Profile
{
    public AdicionalMappingProfile()
    {
        CreateMap<CreateAdicionalRequest, Adicional>();
    }
}