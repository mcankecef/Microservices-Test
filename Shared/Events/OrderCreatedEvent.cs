using Shared.Events.Common;
using Shared.Messages;

namespace Shared.Events;

public class OrderCreatedEvent : IEvent
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Count { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; }
}
