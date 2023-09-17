using ErrorOr;
namespace BuberBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast Not Found");

        public static Error InvalidName => Error.Validation(
            code: "Breakfast.Name",
            description: $"Breakfast name must be at least {Models.Breakfast.MinNameLength} characters long and at most {Models.Breakfast.MaxNameLength} characters long.");

        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.Description",
            description: $"Breakfast description must be at least {Models.Breakfast.MinDescriptionLength} characters long and at most {Models.Breakfast.MaxDescriptionLength} characters long.");

    }
}
