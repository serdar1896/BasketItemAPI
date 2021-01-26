using AutoMapper;
using CicekSepeti.Data.Interface;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.DTOs;
using CicekSepeti.Entities.Models;
using System.Threading.Tasks;

namespace CicekSepeti.Business.Interfaces
{
    public interface IBasketService:IRepository<Basket>
    {
        Task<Result<BasketWithItemResponse>> AddOrSetBasketItemAsync(BasketWithItemDto basketDto);
        Task<Result<BasketWithItemResponse>> GetAllBasketAsync();
        Task<Result<BasketWithItemResponse>> GetByIdBasketAsync(string id);
    }
}
