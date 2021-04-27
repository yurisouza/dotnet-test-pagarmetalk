using AutoMapper;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.Api.Models;
using PagarMeTalk.Api.Models.Output;

namespace PagarMeTalk.Api.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModelOutput>();

            CreateMap<AddItemModel, Item>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PriceInCents, opt => opt.MapFrom(src => src.PriceInCents));

            CreateMap<Item, ItemModelOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PriceInCents, opt => opt.MapFrom(src => src.PriceInCents));
        }
    }
}
