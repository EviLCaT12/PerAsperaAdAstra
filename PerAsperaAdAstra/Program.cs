using DataBase;
using DataBase.Repository;
using Domain.Logic.Intefaces;
using Domain.UseCases;
using PerAsperaAdAstra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;



var confBuilder = new ConfigurationBuilder();
confBuilder.SetBasePath(Directory.GetCurrentDirectory());
confBuilder.AddJsonFile("appsettings.json");
var config = confBuilder.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.EnableSensitiveDataLogging(true));
builder.Services.AddDbContext<ApplicationContext>(ServiceLifetime.Scoped);
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<NodeService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapControllers();

app.Run();