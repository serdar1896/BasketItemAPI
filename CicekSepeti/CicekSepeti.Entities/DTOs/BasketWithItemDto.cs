using System.ComponentModel.DataAnnotations;

namespace CicekSepeti.Entities.DTOs
{
    public class BasketWithItemDto
    {
        [StringLength(24, ErrorMessage = "{0} Format is wrong",MinimumLength =0)]
        public string BasketIdx { get; set; }

        [Required(ErrorMessage = "{0} Can't be null")]
        [StringLength(24, ErrorMessage = "{0} format is wrong", MinimumLength=24)]
        public string CustomerIdx { get; set; }

        [Required(ErrorMessage = "{0} Can't be null")]
        [StringLength(24, ErrorMessage = "{0} Format is wrong", MinimumLength = 24)]
        public string ProductIdx { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "{0} Field must be greater than 1")]
        public int Quantity { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "{0} Field must be greater than 1")]
        public decimal SalePrice { get; set; }
    }
}
