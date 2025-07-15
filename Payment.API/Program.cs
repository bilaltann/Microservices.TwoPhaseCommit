using MassTransit;
using Payment.API.Consumer;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<StockReservedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Payment_StockReservedEventQueue, e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
        


    });

});

var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Payment Service is ready");
    return true;

});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Payment Service is commited");
    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Payment Service is rollbacked");

});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
