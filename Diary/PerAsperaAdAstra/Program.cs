using DataBase;
using DataBase.Repository;
using Domain.Logic.Intefaces;
using Domain.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;




var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DataBaseCfgHelper>(builder.Configuration.GetSection("DataBaseConnectionConfig"));


builder.Services.AddDbContext<ApplicationContext>((serviceProvider, options) =>
{
    var config = serviceProvider.GetRequiredService<IOptions<DataBaseCfgHelper>>().Value;
    var connectionString = $"Host={config.Host};Port={config.Port};Database={config.DataBase};Username={config.Username};Password={config.Password}";
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<NodeService>();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapControllers();

app.Run();