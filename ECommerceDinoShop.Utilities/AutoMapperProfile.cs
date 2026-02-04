using AutoMapper;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, SesionDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>().ForMember(des =>
            des.IdCategoryNavigation,
            opt => opt.Ignore()
            );

            CreateMap<OrderDetail, OrderDetailDTO>()
                 .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.IdProductNavigation.Name))
                 .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.IdProductNavigation.ImageUrl));
            CreateMap<OrderDetailDTO, OrderDetail>();

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.IdUserNavigation.FullName))
                .ForMember(dest => dest.CreatedAtString, opt => opt.MapFrom(src => src.CreatedAt.Value.ToString("dd/MM/yyyy HH:mm")));
            CreateMap<OrderDTO, Order>();            
        }
    }
}
