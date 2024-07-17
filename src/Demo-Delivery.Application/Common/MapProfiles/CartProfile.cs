using AutoMapper;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Domain.Entities.CartAggregate;

namespace Demo_Delivery.Application.Common.MapProfiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Domain.Entities.CartAggregate.Cart, CartViewModel>()
            .ForMember(x => x.CustomerId, 
                opt
                    => opt.MapFrom(src => src.CustomerId))
            
            .ForMember(x => x.CartItems, 
                opt
                    => opt.MapFrom(src => src.CartItems));
        
        CreateMap<CartItem, CartItemViewModel>()
            .ForMember(dest => dest.ProductId, 
                opt
                    => opt.MapFrom(src => src.ProductId))
            
            .ForMember(dest => dest.ProductName, 
                opt
                    => opt.MapFrom(src => src.ProductName))
            
            .ForMember(dest => dest.Price, 
                opt
                    => opt.MapFrom(src => src.UnitPrice))
            
            .ForMember(dest => dest.ImageKey,
                opt
                    => opt.MapFrom(src => src.ImageKey))
            
            .ForMember(dest => dest.Quantity, 
                opt
                    => opt.MapFrom(src => src.Quantity))
            
            .ForMember(dest => dest.TotalPrice, 
                opt => opt.Ignore());
    }
}