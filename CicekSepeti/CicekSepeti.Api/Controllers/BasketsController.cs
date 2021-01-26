using CicekSepeti.Api.Filters;
using CicekSepeti.Business.Interfaces;
using CicekSepeti.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CicekSepeti.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [ValidationFilter]
        [HttpPost("AddOrSetBasketItem")]
        public async Task<IActionResult> AddOrSetBasketItem(BasketWithItemDto basketDto)
        {            
            return Ok(await _basketService.AddOrSetBasketItemAsync(basketDto));
        }

        [HttpGet("BasketItem")]
        public async Task<IActionResult> GetWithBasketItems()
        {
            return Ok(await _basketService.GetAllBasketAsync());
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{id}/BasketItem")]
        public async Task<IActionResult> GetWithBasketItemsById(string id)
        {
            return Ok(await _basketService.GetByIdBasketAsync(id));             
        }

    }
}