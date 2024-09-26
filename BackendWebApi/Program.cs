using BackendWebApi.Data;
using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICategory, RCategory>();
builder.Services.AddScoped<IClassify, RClassify>();
builder.Services.AddScoped<IProducer, RProducer>();
builder.Services.AddScoped<IPersonnel, RPersonnel>();
builder.Services.AddScoped<IWarehouse_Info, RWarehouse_Info>();
builder.Services.AddScoped<IProduct_Info, RProduct_Info>();
builder.Services.AddScoped<ICustomer, RCustomer>();
builder.Services.AddScoped<IWarehouse_Import, RWarehouse_Import>();
builder.Services.AddScoped<IWarehouse_Export, RWarehouse_Export>();
builder.Services.AddScoped<IWarehouse_Data, RWarehouse_Data>();
builder.Services.AddScoped<IDashboard, RDashboard>();
builder.Services.AddScoped<IReportCustomer, RReportCustomer>();
builder.Services.AddScoped<IReportProduct, RReportProduct>();
builder.Services.AddScoped<IReportSale, RReportSale>();
builder.Services.AddScoped<IAdmin_Account, RAdmin_Account>();
builder.Services.AddScoped<JwtService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", p =>
    {
        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Cors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
