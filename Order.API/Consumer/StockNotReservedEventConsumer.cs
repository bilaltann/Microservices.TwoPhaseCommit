using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;

        public StockNotReservedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            OrderAPI.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await _orderAPIDbContext.SaveChangesAsync();

        }
    }


}
