using Coordinator.Migrations;
using Coordinator.Models;
using Coordinator.Models.Contexts;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Coordinator.Models.Enums;
namespace Coordinator.Services
{
    public class TransactionService(IHttpClientFactory _httpClientFactory , TwoPhaseCommitContext _context) : ITransactionService
    {

        HttpClient _orderHttpClient = _httpClientFactory.CreateClient("OrderAPI");
        HttpClient _stockHttpClient = _httpClientFactory.CreateClient("StockAPI");
        HttpClient _paymentHttpClient = _httpClientFactory.CreateClient("PaymentAPI");
        HttpClient _mailHttpClient = _httpClientFactory.CreateClient("MailAPI"); 


        public async Task<Guid> CreateTransactionAsync()
        {
            Guid transactionId= Guid.NewGuid();
            var nodes = await _context.Nodes.ToListAsync();
            nodes.ForEach(node => node.NodeStates = new List<NodeState>
                {
                new(transactionId)
                {
                    IsReady=Models.Enums.ReadyType.Pending,
                    TransactionState=Models.Enums.TransactionState.Pending
                }
                });

            await _context.SaveChangesAsync();
            return transactionId;

        }


        public async Task PrepareServiceAsync(Guid transactionId)
        {
            var transactionNodes= await _context.NodeStates.Include(ns=>ns.Node)
                .Where(ns=>ns.TransactionId==transactionId)
                .ToListAsync();

            foreach (var transactionNode in transactionNodes) 
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "OrderAPI" => _orderHttpClient.GetAsync("ready"),
                        "Stock.API" => _stockHttpClient.GetAsync("ready"),
                        "Payment.API" => _paymentHttpClient.GetAsync("ready"),
                        "Mail.API" => _mailHttpClient.GetAsync("ready")
                    });

                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.IsReady = result ? ReadyType.Ready : ReadyType.Unready;
                }
                catch (Exception ex)
                {
                    transactionNode.IsReady = ReadyType.Unready;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckReadyServiceAsync(Guid transactionId)
            => (await _context.NodeStates
                .Where(ns => ns.TransactionId == transactionId)
                .ToListAsync()).TrueForAll(ns => ns.IsReady == ReadyType.Ready);
             
        
        public async Task CommitAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeStates
                .Where(ns => ns.TransactionId == transactionId)
                .Include(ns => ns.Node)
                .ToListAsync();

            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "OrderAPI" => _orderHttpClient.GetAsync("commit"),
                        "Stock.API" => _stockHttpClient.GetAsync("commit"),
                        "Payment.API" => _paymentHttpClient.GetAsync("commit"),
                        "Mail.API" => _mailHttpClient.GetAsync("commit")
                    });
                    var result =  bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.TransactionState = result ? TransactionState.Done : TransactionState.Abort;


                }
                catch
                {
                    transactionNode.TransactionState = TransactionState.Abort;
                }

                await _context.SaveChangesAsync();
            }
        }


        public async Task<bool> CheckTransactionStateServiceAsync(Guid transactionId)
            => (await _context.NodeStates
            .Where(ns => ns.TransactionId == transactionId)
            .ToListAsync()).TrueForAll(ns => ns.TransactionState == TransactionState.Done);

        
      
        // rollback talimatı eğer 10 işlemden 7'si done diğer 3'ü abort ise , 7 tane done işlemine rollback talimatı verilmeli
        public async Task RollbackAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeStates
                .Include(ns => ns.Node)
                .Where(ns => ns.TransactionId == transactionId)
                .ToListAsync();

            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    if(transactionNode.TransactionState == TransactionState.Done)
                    
                        _ = await (transactionNode.Node.Name switch
                        {
                            "OrderAPI" => _orderHttpClient.GetAsync("rollback"),
                            "Stock.API" => _stockHttpClient.GetAsync("rollback"),
                            "Payment.API" => _paymentHttpClient.GetAsync("rollback"),
                            "Mail.API" => _mailHttpClient.GetAsync("rollback")
                        });
                    
                    transactionNode.TransactionState = TransactionState.Abort;

                }

                catch 
                {
                    transactionNode.TransactionState = TransactionState.Abort;

                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
