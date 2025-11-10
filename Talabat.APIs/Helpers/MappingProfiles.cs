using AutoMapper;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Identity;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // S [Source] ---> Product
            // D [Destination] -----> ProductToReturnDto
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.PictureURL, O => O.MapFrom<ProductPictureUrlResolver>());


            CreateMap<Core.Models.Identity.Address, AddressDto>().ReverseMap();


            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();


            CreateMap<AddressDto, Core.Models.Order_Aggregate.Address>();


            // Destination --> OrderToReturnDto
            // Source --> Order
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            // Destination --> OrderItemDto
            // Source --> OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                //.ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl));
                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
