using Shared.Events.Common;

namespace Shared.Events;

public class StockReservedEvent : IEvent
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public bool IsSuccess { get; set; }
}
