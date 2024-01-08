using MassTransit;
using Shared;
using Shared.Events;
using StockAPI.Models;

namespace StockAPI.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndPoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public OrderCreatedEventConsumer(IPublishEndpoint publishEndPoint,
        ISendEndpointProvider sendEndpointProvider)
    {
        _publishEndPoint = publishEndPoint;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var stockList = new List<Stock>()
        {
            new ()
            {
                OrderId= Guid.Parse("963861b3-5cf8-424f-91d2-6e7841f0f2e7"),
                Count=3,
                ProductId=Guid.Parse("9caf6760-eaa7-4d15-8ccd-34fe7271cee6")
            }
        };

        var stock = stockList.FirstOrDefault(s => s.ProductId == context.Message.OrderItems.Select(oi => oi.ProductId).FirstOrDefault() && s.Count >= context.Message.OrderItems.Select(oi => oi.Count).FirstOrDefault());

        if (stock != null)
        {
            var stockReservedEvent = new StockReservedEvent
            {
                OrderId = stock.OrderId,
                ProductId = stock.ProductId,
                IsSuccess = true
            };

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:" +
                $"{RabbitMQSettings.Payment_StockReservedEventQueue}"));

            await sendEndpoint.Send(stockReservedEvent);

            Console.WriteLine($"{stock.ProductId} kimliğine sahip üründe stock vardır. Satın alıma uygundur.");
        }
        else
        {
            var stockNotReservedEvent = new StockNotReservedEvent
            {
                OrderId = context.Message.OrderId,
                ProductId = context.Message.ProductId,
                IsSuccess = false
            };

            await _publishEndPoint.Publish(stockNotReservedEvent);
        }
    }
}
