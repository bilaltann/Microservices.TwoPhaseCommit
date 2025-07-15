namespace OrderAPI.ViewModels
{
    public class CreateOrderVM
    {
        public Guid BuyerId { get; set; }
        public  string BuyerEmail { get; set; }
        public List<CreateOrderItemVM> OrderItems { get; set; }
    }
}
