using DataBase;
using DataBase.Repository;
using Domain.Logic.Intefaces;
using Domain.UseCases;
using PerAsperaAdAstra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql($"Host=localhost;Port=5432;Database=diary;Username=postgres;Password=qwerty123"));
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.EnableSensitiveDataLogging(true));
builder.Services.AddTransient<INodeRepository, NodeRepository>();
builder.Services.AddTransient<NodeService>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();