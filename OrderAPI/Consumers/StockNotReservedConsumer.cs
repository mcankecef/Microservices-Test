using MassTransit;
using OrderAPI.Models;
using Shared.Events;

namespace OrderAPI.Consumers;

public class StockNotReservedConsumer : IConsumer<StockNotReservedEvent>
{
    public Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        var order = OrderList.Orders.FirstOrDefault(o => o.Id == context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Pending;

            Console.WriteLine($"{order.Id} kimliğe sahip sipariş geçersiz stok sebebiyle bekleme aşamasındadır lütfen geçerli stok adetinde alınız. İlgili birimle iletişime geçiniz! ({order.Status})");
        }

        return Task.CompletedTask;
    }
}
