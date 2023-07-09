namespace api_pim.Mappers;

using AutoMapper;
using api_pim.Models;
using api_pim.Entities;

public class EmpresaMappingProfile : Profile
{
    public EmpresaMappingProfile()
    {
        CreateMap<CreateEmpresaRequest, Empresa>();
    }
}