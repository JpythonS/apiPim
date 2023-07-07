using AutoMapper;

using api_pim.Models;
using api_pim.Entities;

namespace api_pim.Mappers;

public class UsuarioMappingProfile : Profile
{
    public UsuarioMappingProfile()
    {
        CreateMap<CreateUsuarioRequest, Usuario>();
    }
}