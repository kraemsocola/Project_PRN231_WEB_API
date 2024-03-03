using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Models;

namespace Project_API.Map
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            
        }
    }
}
