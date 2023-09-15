using BuberBreakfast.Services;

var builder = WebApplication.CreateBuilder(args);
{ 
// Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();

}


var app = builder.Build();


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
