using MassTransit;
using PaymentAPI.Models;
using Shared.Events;

namespace PaymentAPI.Consumers;

public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        var payment = new Payment();

        payment.Id = Random.Shared.Next();

        if (context.Message.IsSuccess)
        {
            payment.IsSuccess = true;

            var paymentCompletedEvent = new PaymentCompletedEvent { OrderId = context.Message.OrderId };

            await _publishEndpoint.Publish(paymentCompletedEvent);
        }
        else
        {
            payment.IsSuccess = false;
            var paymentFailedEvent = new PaymentFailedEvent
            {
                OrderId = context.Message.OrderId,
                Message = $"{context.Message.OrderId} kimliğine sahip ödeme işlemi başarısız."
            };

            await _publishEndpoint.Publish(paymentFailedEvent);
        }
    }
}
