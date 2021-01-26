using CicekSepeti.Data.Interface;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.Models;
using System.Threading.Tasks;

namespace CicekSepeti.Business.Interfaces
{
    public interface IBasketServiceHelper
    {
        Task<BasketWithItemResponse> ToBasketModelAsync(Basket basket);
        Task<Result<BasketWithItemResponse>> AddOrSetBasketItem(BasketItem basketItem, Basket basket);
        Task<Basket> AddOrGetBasket(Basket vmBasket);
        bool IsThereStock(string productIdx, int quantity);
    }
}
