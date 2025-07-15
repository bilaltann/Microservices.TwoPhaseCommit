using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Consumer;
using OrderAPI.Models.Entities;
using Shared;

var builder = WebApplication.CreateBuilder(args);




// MassTransit ve DbContext yapýlandýrmalarý
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<PaymentEventCompletedEventConsumer>();
    configurator.AddConsumer<StockNotReservedEventConsumer>();
    configurator.AddConsumer<PaymentFailedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentEventCompletedEventConsumer>(context));
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotEventQueue, e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
        _configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});
builder.Services.AddDbContext<OrderAPIDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});


var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Order Service is ready");
    return true;

});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Order Service is commited");
    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Order Service is rollbacked");

});


app.UseRouting();



app.Run();