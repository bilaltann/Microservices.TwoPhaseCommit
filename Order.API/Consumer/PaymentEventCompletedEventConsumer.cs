using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class PaymentEventCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;
        readonly IPublishEndpoint _publishEndpoint;

        public PaymentEventCompletedEventConsumer(OrderAPIDbContext orderAPIDbContext, IPublishEndpoint publishEndpoint)
        {
            _orderAPIDbContext = orderAPIDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);

            if (order == null)
            {
                // Sipariş bulunamadıysa işlem yapılmasın
                return;
            }

            order.OrderStatus = Models.Enums.OrderStatus.Completed;

            await _orderAPIDbContext.SaveChangesAsync(); // önce kaydet

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = order.OrderId,
                BuyerId = order.BuyerId,
                BuyerEmail = order.BuyerEmail,
                Message = "Order has been completed successfully."
            };

            await _publishEndpoint.Publish(orderCompletedEvent); // sonra publish et
        }
    }
}
