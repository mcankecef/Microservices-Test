namespace StockAPI.Models;

public class Stock
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Count { get; set; }
}
