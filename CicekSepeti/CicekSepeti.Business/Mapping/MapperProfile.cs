using AutoMapper;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.DTOs;
using CicekSepeti.Entities.Models;

namespace CicekSepeti.Business.Mapping
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<BasketItem, BasketWithItemDto>();
            CreateMap<BasketWithItemDto, BasketItem>();

            CreateMap<Basket, BasketWithItemDto>().ForMember(des => des.BasketIdx, opt => opt.MapFrom(src => src.Id));
            CreateMap<BasketWithItemDto, Basket>().ForMember(des => des.Id, opt => opt.MapFrom(src => src.BasketIdx));

            CreateMap<Result<BasketItem>, Result<BasketWithItemDto>>();
            CreateMap<Result<BasketWithItemDto>, Result<BasketItem>>();

        }
    }
}
