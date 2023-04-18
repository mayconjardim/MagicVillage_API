using AutoMapper;
using MagicVillage_API.Model;
using MagicVillage_API.Model.Dto;

namespace MagicVillage_API.Profiles
{
    public class MappingConfig : Profile
    {

        public MappingConfig() 
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

        }


    }
}
