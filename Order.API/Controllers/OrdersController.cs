using Microsoft.AspNetCore.Mvc;
using OrderAPI.ViewModels;
using OrderAPI.Models.Entities;
using MassTransit;
using Shared.Events;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(OrderAPIDbContext context ,IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            OrderAPI.Models.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId=createOrder.BuyerId,
                BuyerEmail=createOrder.BuyerEmail,
                CreatedDate = DateTime.Now,
                OrderStatus = Models.Enums.OrderStatus.Suspend
            };

            order.OrderItems=createOrder.OrderItems.Select(oi=> new OrderItem
            {
                Count= oi.Count,
                Price= oi.Price,
                ProductId=oi.ProductId,
            }).ToList();
            order.TotalPrice=createOrder.OrderItems.Sum(oi=>(oi.Price*oi.Count));
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            OrderCreatedEvent orderCreatedEvent = new()
            {
                BuyerId = order.BuyerId,
                OrderId = order.OrderId,
                
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
                {
                    ProductId = oi.ProductId,
                    Count = oi.Count,
                }).ToList(),
                TotalPrice =order.TotalPrice
            };

            await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok();
        }
    }
}