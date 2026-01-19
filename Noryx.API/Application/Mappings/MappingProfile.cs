using AutoMapper;
using Noryx.API.Application.Dtos;
using Noryx.API.Models;

namespace Noryx.API.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Moeda, MoedaDto>().ReverseMap();
        }
    }
}
