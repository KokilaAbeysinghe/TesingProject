using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Application.Services;
using TestingProject.Infrastructure.Data;
using TestingProject.Infrastructure.Repositories;
using TestingProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration
        .GetConnectionString("DefaultConnection")));


// register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();