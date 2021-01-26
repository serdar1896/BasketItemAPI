using CicekSepeti.Business.Interfaces;
using CicekSepeti.Data.Interface;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.ComplexTypes.Enum;
using CicekSepeti.Entities.Models;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepeti.Business.Helper
{
    public class BasketServiceHelper:IBasketServiceHelper
    {
        private readonly IRepository<BasketItem> _basketItemRepository;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<Stock> _stockRepository;

        public BasketServiceHelper(
            IRepository<Basket> basketRepository,
            IRepository<BasketItem> basketItemRepository,
            IRepository<Stock> stockRepository
            )
        {
            _basketItemRepository = basketItemRepository;
            _basketRepository = basketRepository;
            _stockRepository = stockRepository;

        }
        public async Task<BasketWithItemResponse> ToBasketModelAsync(Basket basket)
        {
            var vmBasket = new BasketWithItemResponse();
            var basketItems = await _basketItemRepository.GetByParamAsync(x => x.BasketIdx == basket.Id);
            vmBasket.Id = basket.Id;
            vmBasket.CustomerIdx = basket.CustomerIdx;
            vmBasket.CreatedDate = basket.CreatedDate;
            vmBasket.BasketItem = basketItems.ToList();
            return vmBasket;
        }
        public async Task<Result<BasketWithItemResponse>> AddOrSetBasketItem(BasketItem basketItem, Basket basket)
        {
            var result = new Result<BasketWithItemResponse>();

            var customerBasketItem = await _basketItemRepository.GetByParamAsync(x => x.BasketIdx == basket.Id);
            var bufferItem = customerBasketItem.FirstOrDefault(cb => cb.ProductIdx == basketItem.ProductIdx);

            if (bufferItem == null)
            {
                basketItem.BasketIdx = basket.Id;
                 await _basketItemRepository.AddAsync(basketItem);
                result.IsSuccessful = true;
                result.EntityList.Add(await ToBasketModelAsync(basket));
            }
            else
            {
                bufferItem.SalePrice = basketItem.SalePrice;
                bufferItem.Quantity += basketItem.Quantity;
                if (IsThereStock(bufferItem.ProductIdx, bufferItem.Quantity))
                {
                    await _basketItemRepository.UpdateAsync(bufferItem.Id, bufferItem);
                    result.IsSuccessful = true;
                    result.EntityList.Add(await ToBasketModelAsync(basket));
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessages.Add(ErrorCodes.OutOfStock);
                }
            }
            return result;
        }
        public bool IsThereStock(string productIdx, int quantity)
        {
            //#region Stok kontrolünü test etmek için ürüne stok ekledik.
            //_stockRepository.AddAsync(new Stock()
            //{
            //    ProductIdx = productIdx,
            //    Quantity = 10,
            //    Type = 1,
            //    WarehouseAddress = "Istanbul",
            //    UpdatedDate = DateTime.Now
            //}); 
            //#endregion

            return _stockRepository.GetByParamAsync(x => x.ProductIdx == productIdx && x.Quantity >= quantity).Result.FirstOrDefault() != null ? true : false;
        }
        public async Task<Basket> AddOrGetBasket(Basket vmBasket)
        {

            if (!ObjectId.TryParse(vmBasket.Id, out ObjectId objectId))
            {
                vmBasket.Id = objectId.ToString();
            }
            var basket = _basketRepository.GetByParamAsync(x => x.Id == vmBasket.Id && x.CustomerIdx == vmBasket.CustomerIdx).Result.FirstOrDefault();
            if (basket == null)
            {
                await _basketRepository.AddAsync(new Basket()
                {
                    CustomerIdx = vmBasket.CustomerIdx,
                    CreatedDate = DateTime.Now
                });
                var customerBasket = await _basketRepository.GetByParamAsync(basket => basket.CustomerIdx == vmBasket.CustomerIdx);
                basket = customerBasket.FirstOrDefault();
            }
            return basket;
        }
    }
}
