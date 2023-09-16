
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.BreakfastService;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController]
//[Route("breakfast")]
//OR:
[Route("[controller]")]
public class BreakfastController : ControllerBase
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
            Guid.NewGuid(), request.Name, request.Description, request.StartDateTime, request.EndDateTime, DateTime.UtcNow,
            request.Savory, request.Sweet);

        _breakfastService.CreateBreakfast(breakfast);

        var response = new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime,
            breakfast.LastModifiedDateTime, breakfast.Savory, breakfast.Sweet);

        //return Ok(request); --> this returns 200. But we want 201 (Created)
        //there are few options for this like Created(), CreatedAtRoute(), CreatedAtAction(), etc. We'll use CreatedAtAction()
        //This will allow us to pass the info in response which user can use to retrieve the newly created resource
        return CreatedAtAction(
            nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: response
            );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        var getBreakfastResult = _breakfastService.GetBreakfast(id);

        if (getBreakfastResult.IsError && getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
            return NotFound();

        var breakfast = getBreakfastResult.Value;
        var response = new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime,
            breakfast.LastModifiedDateTime, breakfast.Savory, breakfast.Sweet);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id, request.Name, request.Description, request.StartDateTime, request.EndDateTime, DateTime.UtcNow,
            request.Savory, request.Sweet);

        _breakfastService.UpsertBreakfast(breakfast);

        //TODO: If new, return 201, like we are doing in Create method.


        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        _breakfastService.DeleteBreakfast(id);
        return NoContent();
    }
}
