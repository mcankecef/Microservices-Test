using MassTransit;
using OrderAPI.Models;
using Shared.Events;

namespace OrderAPI.Consumers;

public class PaymentCompletedConsumer : IConsumer<PaymentCompletedEvent>
{
    public Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var order = OrderList.Orders.FirstOrDefault(o => o.Id == context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Succeed;

            Console.WriteLine($"{order.Id} kimliğe sahip sipariş başarılı bir şekilde tamamlandı. ({order.Status})");
        }

        return Task.CompletedTask;
    }
}
