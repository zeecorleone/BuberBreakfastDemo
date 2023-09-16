using BuberBreakfast.Models;
using ErrorOr;

namespace BuberBreakfast.Services.BreakfastService
{
    public interface IBreakfastService
    {
        ErrorOr<Created> CreateBreakfast(Breakfast request);
        ErrorOr<Deleted> DeleteBreakfast(Guid id);
        ErrorOr<Breakfast> GetBreakfast(Guid id);
        ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast);
    }
}
