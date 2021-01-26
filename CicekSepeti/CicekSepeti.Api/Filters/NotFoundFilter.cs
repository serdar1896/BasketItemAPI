using CicekSepeti.Business.Interfaces;
using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.ComplexTypes.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepeti.Api.Filters
{
    public class NotFoundFilter:ActionFilterAttribute
    {
        private readonly IBasketService _basketService;
        public NotFoundFilter(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string id = context.ActionArguments.Values.FirstOrDefault()?.ToString()??null;
            var result = new Result<string>();
            if (!ObjectId.TryParse(id, out ObjectId _))
            {
                result.ErrorMessages.Add(ErrorCodes.IdFormatIsWrong);
                context.Result = new NotFoundObjectResult(result);
            }
            else
            {
                var basket = await _basketService.GetByIdAsync(id);
                if (basket == null)
                {
                    result.ErrorMessages.Add(ErrorCodes.NoRecordById);
                    context.Result = new NotFoundObjectResult(result);
                }
                else await next();
            }

        }
    }
}
