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

            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetail>();

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
        }
    }
}
