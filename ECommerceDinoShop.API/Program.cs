using ECommerceDinoShop.Repository; //Para poder acceder a la cadena de conexion con la DB
using Microsoft.EntityFrameworkCore; //Para poder acceder a la cadena de conexion con la DB


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbdinoShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection")); //Para poder acceder a la cadena de conexion con la DB
});

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

