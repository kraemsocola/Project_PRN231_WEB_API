using AutoMapper;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Category;
using BussinessObjects.Dto.OrderDetail;
using BussinessObjects.Dto.Product;
using BussinessObjects.Models;

namespace Project_API.Map
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Album, CreateAlbumDto>().ReverseMap();
            CreateMap<Album, UpdateAlbumDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>()
                .ForMember(x=>x.CategoryName, y=>y.MapFrom(src => src.Category.Name))   
                .ReverseMap();
            CreateMap<OrderDetail, CreateOrderDetailDto>().ReverseMap();
            CreateMap<OrderDetail, UpdateOrderDetailDto>().ReverseMap();
        }
    }
}
