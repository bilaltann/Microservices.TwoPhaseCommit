

using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Models.Entities;

public class OrderAPIDbContext: DbContext
{
    public OrderAPIDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
