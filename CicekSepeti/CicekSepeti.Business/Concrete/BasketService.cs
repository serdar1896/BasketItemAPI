using AutoMapper;
using CicekSepeti.Business.Interfaces;
using CicekSepeti.Data.Concrete;
using CicekSepeti.Data.Interface;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.ComplexTypes.Enum;
using CicekSepeti.Entities.DTOs;
using CicekSepeti.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CicekSepeti.Business.Concrete
{
    public class BasketService : BaseRepository<Basket>, IBasketService
    {
        private readonly ICacheService _redisService;
        private readonly IBasketServiceHelper _basketServiceHelper;
        private readonly IMapper _mapper;
        public BasketService(
            ICacheService redisService,
            IBasketServiceHelper basketServiceHelper,
            ICicekSepetiDatabaseSettings settings,
            IMapper mapper

            ) : base(settings)
        {
            _redisService = redisService;
            _basketServiceHelper = basketServiceHelper;
            _mapper=mapper;
    }


        #region AddProductToBasketAsync
        /* TODO: AddProductToBasketAsync;
         * Gelen model, basket ve basketItem modeline mapleniyor.
         * Sepete eklenmek istenen ürün ve miktarı Stokta var mı diye kontrol ediliyor.
         * Stokta var ise Sepete ekleme yapan müşteriye ait eğer varsa tüm sepet getiriliyor. 
         * Yoksa yeni bir sepet oluşturulup getiriliyor.
         * Sonrasında sepete eklenen ürün daha önce sepette eklenmiş mi diye kontrol ediliyor
         * Daha önce eklenmemiş ise yeni bir basket item ekleniyor.
         * Daha önce eklenmiş ise daha önce eklenen ürün miktarı ile yeni eklenmek istenen ürün miktarı toplanıp Stok kontrolü yapılıyor 
         * Ve stokta var ise ürünün miktarı güncelleniyor. Stokta yok ise Stok'un yetersiz olduğunu belirten hata döndürülüyor.
         * Tüm işlemler başarılı olursa, müşteriye ait sepetin cache'i Redisden temizleniyor. 
         * En son geriye İşlemler başarılı olduysa eklenen veya güncellenen sepet döndürülüyor.
         * Başarısız olursa ilgili hata döndürülüyor.
         */
        public async Task<Result<BasketWithItemResponse>> AddOrSetBasketItemAsync(BasketWithItemDto basketDto)
        {
            var result = new Result<BasketWithItemResponse>();
            try
            {
                var vmBasket = _mapper.Map<Basket>(basketDto);
                var vmBasketItem = _mapper.Map<BasketItem>(basketDto);

                if (_basketServiceHelper.IsThereStock(vmBasketItem.ProductIdx, vmBasketItem.Quantity))
                {
                    var basket = await _basketServiceHelper.AddOrGetBasket(vmBasket);
                    result= await _basketServiceHelper.AddOrSetBasketItem(vmBasketItem, basket);
                    await _redisService.RemoveByPatternAsync($"basket{basket.Id}");
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessages.Add(ErrorCodes.OutOfStock);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.ErrorMessages.Add( new ErrorModel { Code = 500, Text = ex.Message });

            }

            return result;
        }
        #endregion

        #region GetByIdAsync;
        /* TODO: GetByIdAsync;
         * Çağırılan sepet cache'de var ise itemlarıyla birlikte db'ye gitmeden getiriliyor.
         * Cache'de yok ise dbden getirilip VmBasket modeline maplenip cache'e ekleniyor.
         * En son geriye işlemin sonucu ve istenen model döndürülüyor.
         */
        public async Task<Result<BasketWithItemResponse>> GetByIdBasketAsync(string id)
        {
            var result = new Result<BasketWithItemResponse>();
            try
            {
                var cacheResult = await _redisService.GetAsync<BasketWithItemResponse>($"basket{id}");
                if (cacheResult != null)
                {
                    result.EntityList.Add(cacheResult);
                    result.IsSuccessful = true;
                    return result;
                }

                var basket = await base.GetByIdAsync(id);
                var vmBasket = await _basketServiceHelper.ToBasketModelAsync(basket);

                result.EntityList.Add(vmBasket);
                result.IsSuccessful = true;
                await _redisService.AddAsync($"basket{vmBasket.Id}", vmBasket);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.ErrorMessages.Add(new ErrorModel { Code=500, Text= ex.Message });
            }
            return result;
        }
        #endregion


        #region Sepetlerin Id bilgilerine erişip test edebilmek için
        public async Task<Result<BasketWithItemResponse>> GetAllBasketAsync()
        {
            var result = new Result<BasketWithItemResponse>();
            var vmBasketList = new List<BasketWithItemResponse>();

            var baskets = await base.GetAllAsync();
            foreach (var basket in baskets)
            {
                var vmBasket = await _basketServiceHelper.ToBasketModelAsync(basket);
                vmBasketList.Add(vmBasket);
            }
            result.EntityList = vmBasketList;
            return result;
        } 
        #endregion

    }

}
