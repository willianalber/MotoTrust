using MassTransit;
using MotoTrust.Application;
using MotoTrust.Consumer.Consumers;
using MotoTrust.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MotoTrust API", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MotoTrust.API.xml"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MotorcycleCreatedConsumer>();
    x.AddConsumer<MotorcycleRentedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitConfig = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbitConfig["Host"], "/", h =>
        {
            h.Username(rabbitConfig["Username"]);
            h.Password(rabbitConfig["Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoTrust API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Iniciando MotoTrust API - sistema de locação de motos");
    app.Run();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Aplicação terminou inesperadamente");
}