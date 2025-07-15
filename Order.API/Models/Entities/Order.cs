using OrderAPI.Models.Enums;

namespace OrderAPI.Models.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerEmail { get; set; }
        public decimal TotalPrice {  get; set; } 
        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
