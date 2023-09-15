﻿using BuberBreakfast.Models;

namespace BuberBreakfast.Services
{
    public interface IBreakfastService
    {
        void CreateBreakfast(Breakfast request);
        void DeleteBreakfast(Guid id);
        Breakfast GetBreakfast(Guid id);
        void UpsertBreakfast(Breakfast breakfast);
    }
}
