using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers;

[ApiController]
//[Route("breakfast")]
//OR:
[Route("[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {

        //if all errors are validation errors, then return beutiful ModelStateDictonary with appropriate errors
        if(errors.All(x => x.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
                modelStateDictionary.AddModelError(error.Code, error.Description);
            return ValidationProblem(modelStateDictionary);
        }

        //handle any unexpected errors 
        if (errors.Exists(x => x.Type == ErrorType.Unexpected))
            return Problem();

        //for others/handled
        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}
