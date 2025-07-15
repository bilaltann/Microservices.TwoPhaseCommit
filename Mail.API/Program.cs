using Mail.API.Services;
using Mail.API.Settings;
using Shared;
using Shared.Events;
using MassTransit;
using Mail.API.Consumer;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCompletedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);
        _configurator.ReceiveEndpoint(RabbitMQSettings.Mail_OrderCompletedEventQueue, e => e.ConfigureConsumer<OrderCompletedEventConsumer>(context));
    });
});

var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Mail Service is ready");
    return true;

});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Mail Service is commited");
    return true;

});

app.MapGet("/rollback", () =>
{
    Console.WriteLine("Mail Service is rollbacked");

});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




app.Run();
