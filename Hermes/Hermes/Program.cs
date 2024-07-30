using DataBase;
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