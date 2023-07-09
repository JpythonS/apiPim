namespace api_pim.Mappers;

using AutoMapper;
using api_pim.Models;
using api_pim.Entities;

public class FuncionarioMappingProfile : Profile
{
    public FuncionarioMappingProfile()
    {
        CreateMap<CreateFuncionarioRequest, Funcionario>()
        .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}