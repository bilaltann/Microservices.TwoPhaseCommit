using MassTransit;
using Shared.Events;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //ödeme işlemleri
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                _publishEndpoint.Publish(paymentCompletedEvent);
                Console.WriteLine("ödeme başarılı");
            }

            else
            {
                //ödemede sıkıntı olduğunu 
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "bakiye yetersiz"
                };
                _publishEndpoint.Publish(paymentFailedEvent);
                Console.WriteLine("ödeme başarısız");

            }
            return Task.CompletedTask;
        }
    }
}
