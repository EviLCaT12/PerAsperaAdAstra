using DataBase;
using Hermes.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Hermes;
using Confluent.Kafka;
using Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DataBaseCfgHelper>(builder.Configuration);
builder.Services.Configure<DataBaseCfgHelper>(builder.Configuration.GetSection("DataBaseConnectionConfig"));

builder.Services.AddDbContext<ApplicationContext>((serviceProvider, options) =>
{
    var config = serviceProvider.GetRequiredService<IOptions<DataBaseCfgHelper>>().Value;
    var connectionString = $"Host={config.Host};Port={config.Port};Database={config.DataBase};Username={config.Username};Password={config.Password}";
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddSingleton(sp =>
{
    var config = new ProducerConfig { BootstrapServers = "localhost:9092 " };
    return new Producer<Null, string>(config);
});

builder.Services.AddSingleton(sp =>
{
    var config = new ConsumerConfig
    {
        GroupId = "test-consumer-group",
        BootstrapServers = "localhost:9092",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new Consumer<Ignore, string>(config);
});

builder.Services.AddHostedService<KafkaConsumerHostedService<Ignore, string>>();

builder.Services.AddHostedService<OutboxService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.Run();
