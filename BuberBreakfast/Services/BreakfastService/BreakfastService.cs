using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.BreakfastService;

public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

    public void CreateBreakfast(Breakfast request)
    {
        _breakfasts.Add(request.Id, request);
    }

    public void DeleteBreakfast(Guid id)
    {
        _breakfasts.Remove(id);
    }

    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if(_breakfasts.TryGetValue(id, out var breakfast))
            return breakfast;
        return Errors.Breakfast.NotFound;
    }

    public void UpsertBreakfast(Breakfast breakfast)
    {
        _breakfasts[breakfast.Id] = breakfast;
    }
}
