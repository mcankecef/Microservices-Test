using MassTransit;
using OrderAPI.Consumers;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ-Configurations
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<StockNotReservedConsumer>();

    configurator.AddConsumer<PaymentCompletedConsumer>();
    configurator.AddConsumer<PaymentFailedConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        // appsetting.json rabbitmq conn
        _configurator.Host(builder.Configuration["RabbitMQ"]);

        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotReservedEventQueue, e =>
            e.ConfigureConsumer<StockNotReservedConsumer>(context)
        );
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e =>
            e.ConfigureConsumer<PaymentCompletedConsumer>(context)
        );
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e =>
            e.ConfigureConsumer<PaymentFailedConsumer>(context)
        );
    });
});

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
