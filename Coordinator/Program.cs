using Coordinator.Models.Contexts;
using Coordinator.Services;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TwoPhaseCommitContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress= new("https://localhost:7191/"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7179/"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7010/"));
builder.Services.AddHttpClient("MailAPI", client => client.BaseAddress = new("https://localhost:7276/"));

builder.Services.AddScoped<ITransactionService , TransactionService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/create-order-transaction", async (ITransactionService transactionService) =>
{
    // 1 .adým  - Prepare
    var transactionId = await transactionService.CreateTransactionAsync();
    await transactionService.PrepareServiceAsync(transactionId);
    bool transactionState = await transactionService.CheckReadyServiceAsync(transactionId);

    if (transactionState)
    {
        // 2.adým - Commit
        await transactionService.CommitAsync(transactionId);
        transactionState = await transactionService.CheckTransactionStateServiceAsync(transactionId);
    }
    if (!transactionState)
    {
        await transactionService.RollbackAsync(transactionId);
    }

});

app.Run();
