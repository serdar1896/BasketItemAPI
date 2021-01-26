using CicekSepeti.Entities.ComplexTypes;
using CicekSepeti.Entities.ComplexTypes.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepeti.Api.Filters
{
    public class ValidationFilter:ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Result<string>();
                IEnumerable <ModelError> modelErrors= context.ModelState.Values.SelectMany(v => v.Errors);
                modelErrors.ToList().ForEach(x =>
                {
                    result.ErrorMessages.Add(new ErrorModel { Code = 400, Text =x.ErrorMessage } );
                });

                context.Result = new BadRequestObjectResult(result);
            }
            else await next();
        }

    }
}
