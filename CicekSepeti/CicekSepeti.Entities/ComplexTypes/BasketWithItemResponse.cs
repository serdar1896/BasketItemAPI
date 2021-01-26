using CicekSepeti.Entities.Models;
using System.Collections.Generic;

namespace CicekSepeti.Entities.ComplexTypes
{
    public class BasketWithItemResponse:Basket
    {
        public BasketWithItemResponse()
        {
            BasketItem = new List<BasketItem>();
        }
        public List<BasketItem> BasketItem { get; set; }
    }
}
