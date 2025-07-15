using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;

        public PaymentFailedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            OrderAPI.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus =Models.Enums.OrderStatus.Failed;
            await _orderAPIDbContext.SaveChangesAsync();
        }
    }
}
