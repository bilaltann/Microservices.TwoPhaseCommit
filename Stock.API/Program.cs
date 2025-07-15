using MassTransit;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        
    });

});

builder.Services.AddSingleton<MongoDBService>();

#region Harici - MongoDB'ye Seed Data Ekleme
using IServiceScope scope= builder.Services.BuildServiceProvider().CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
if (!collection.FindSync(Builders<Stock.API.Models.Entities.Stock>.Filter.Empty).Any())

{
    await collection.InsertOneAsync(new() { ProductId = "5f559722-7cd4-4c5a-bc6e-252d28bc5ec3", Count = 2000 });
    await collection.InsertOneAsync(new() { ProductId = "8e9f559d-b7fa-4a7d-a64d-d7ed8ab587f2", Count = 1000 });
    await collection.InsertOneAsync(new() { ProductId = "a2fccc9e-3ae2-4fd3-8008-0a27911d1cd9", Count = 3000 });
    await collection.InsertOneAsync(new() { ProductId = "b4515011-b582-4f2c-a0a3-3756e5c61010", Count = 5000 });
    await collection.InsertOneAsync(new() { ProductId = "14fb1064-3668-47d0-8f19-36c981fcdd1c", Count = 500 });

}
#endregion







var app = builder.Build();


app.MapGet("/ready", () =>
{
    Console.WriteLine("Stock Service is ready");
    return true;

});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Stock Service is commited");
    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Stock Service is rollbacked");

});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
