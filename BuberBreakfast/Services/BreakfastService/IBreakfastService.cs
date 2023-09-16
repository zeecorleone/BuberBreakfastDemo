﻿using BuberBreakfast.Models;
using ErrorOr;

namespace BuberBreakfast.Services.BreakfastService
{
    public interface IBreakfastService
    {
        void CreateBreakfast(Breakfast request);
        void DeleteBreakfast(Guid id);
        ErrorOr<Breakfast> GetBreakfast(Guid id);
        void UpsertBreakfast(Breakfast breakfast);
    }
}
