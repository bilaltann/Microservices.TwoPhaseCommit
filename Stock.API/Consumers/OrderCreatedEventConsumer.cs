using MassTransit;
using MassTransit.Transports;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent> // OrderCreatedEvent türünden bir event geldiğinde yakala ve aşağıdaki işlemleri yap

    {
        IMongoCollection<Stock.API.Models.Entities.Stock> _stockCollection;
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(MongoDBService mongoDBService ,IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider )
        {
            _stockCollection = mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }
      
       
        public  async Task Consume(ConsumeContext<OrderCreatedEvent> context)
         {
            List<bool> stockResult = new();
            foreach (OrderItemMessage orderItem in context.Message.OrderItems) 
            {
                stockResult.Add((await _stockCollection.FindAsync(s=>s.ProductId == orderItem.ProductId && s.Count>=orderItem.Count)).Any());
                //s → Stock koleksiyonundaki her bir belgeyi (document) temsil eder.
                //Stock.API.Models.Entities.Stock sınıfından gelir.
                //Bir koleksiyonda en az bir tane eleman varsa true, hiç yoksa false döner.

            }

            if (stockResult.TrueForAll(sr=>sr.Equals(true)))
            {
                foreach(OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Stock.API.Models.Entities.Stock stock = await (await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                    stock.Count-=orderItem.Count;
                    await _stockCollection.FindOneAndReplaceAsync(s=>s.ProductId==orderItem.ProductId,stock);
                }

                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice
                };
                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                await sendEndpoint.Send(stockReservedEvent);
                await Console.Out.WriteLineAsync("stok işlemleri başarılı");
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "...."
                };

                await _publishEndpoint.Publish(stockNotReservedEvent);
                await Console.Out.WriteLineAsync("stok işlemleri başarısız");

                // siparişin tutarsız/gecersiz olduğuna dair işlem 
            }
            //return Task.CompletedTask;
        }
    }
}



