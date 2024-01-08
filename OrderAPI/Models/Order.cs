namespace OrderAPI.Models;

public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Order Order { get; set; }
}

public enum OrderStatus
{
    Started,
    Succeed,
    Pending,
    Failed
}

public static class OrderList
{
    // Statik bir liste tanımla ve başlangıç verileri ile doldur
    public static List<Order> Orders { get; } = new List<Order>
    {
        new()
        {
            Id = Guid.Parse("963861b3-5cf8-424f-91d2-6e7841f0f2e7"),
            Status = OrderStatus.Started,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { Id = Guid.Parse("9caf6760-eaa7-4d15-8ccd-34fe7271cee6"), Name = "test-order-item-1", Price = 40, Stock = 1 },
            },
        },
        
    };
}

