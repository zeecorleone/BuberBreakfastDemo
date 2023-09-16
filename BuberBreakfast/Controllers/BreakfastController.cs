
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.BreakfastService;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;


public class BreakfastController : ApiControllerBase
{

    private readonly IBreakfastService _breakfastService;

    public BreakfastController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            Guid.NewGuid(), request.Name, request.Description,
            request.StartDateTime, request.EndDateTime, DateTime.UtcNow,
            request.Savory, request.Sweet);

        ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

        return createBreakfastResult.Match(
                created => CreatedResponse(breakfast),
                errors => Problem(errors)
            );
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
       
        return getBreakfastResult.Match(
                brekfast => Ok(MapBreakfastResponse(brekfast)),
                errors => Problem(errors)
            );


        //Below code not needed anymore, after we:
        //1: used getBreakfastResult.Match() method
        //2: created and used our own base controller "ApiControllerBase" and added Problem() method in it

        //if (getBreakfastResult.IsError && getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        //    return NotFound();

        //var breakfast = getBreakfastResult.Value;
        //BreakfastResponse response = MapBreakfastResponse(breakfast);

        //return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id, request.Name, request.Description, request.StartDateTime, request.EndDateTime, DateTime.UtcNow,
            request.Savory, request.Sweet);

        ErrorOr<UpsertedBreakfast> upsertedResult = _breakfastService.UpsertBreakfast(breakfast);

        //If new, return 201, like we are doing in Create method.
        return upsertedResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedResponse(breakfast) : NoContent(),
                errors => Problem(errors)
            );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        var deleteResult = _breakfastService.DeleteBreakfast(id);

        return deleteResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
    }


    private IActionResult CreatedResponse(Breakfast breakfast)
    {


        //return Ok(request); --> this returns 200. But we want 201 (Created)
        //there are few options for this like Created(), CreatedAtRoute(), CreatedAtAction(), etc. We'll use CreatedAtAction()
        //This will allow us to pass the info in response which user can use to retrieve the newly created resource
        return CreatedAtAction(
            nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastResponse(breakfast)
            );
    }

    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id, 
            breakfast.Name, 
            breakfast.Description, 
            breakfast.StartDateTime, 
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime, 
            breakfast.Savory, 
            breakfast.Sweet);
    }

}
