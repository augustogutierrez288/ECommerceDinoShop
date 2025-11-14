using ECommerceDinoShop.Repository; //Para poder acceder a la cadena de conexion con la DB
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Repository.Implementation;
using ECommerceDinoShop.Service.Contract;
using ECommerceDinoShop.Service.Implementation;
using ECommerceDinoShop.Utilities;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection DB
builder.Services.AddDbContext<DbdinoShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection")); //Para poder acceder a la cadena de conexion con la DB
});

// Add Transient Generic Repository
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//New policy cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
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

app.UseCors("NewPolicy");

app.Run();

