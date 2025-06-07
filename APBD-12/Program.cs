using APBD_12.Data;
using APBD_12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddDbContext<MasterContext>( opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<ITripService, TripService>();
var app = builder.Build();


app.MapControllers();


app.Run();

