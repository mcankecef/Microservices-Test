using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using Shared.Events;

namespace OrderAPI.Controllers;

public class OrdersController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndPoint;

    public OrdersController(IPublishEndpoint publishEndPoint)
        => _publishEndPoint = publishEndPoint;

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
       
        //order.TotalPrice = OrderList.Orders.Select(o => o.OrderItems).Sum(oi=>oi.Select(oi=>oi.Price).FirstOrDefault());
        //order.Status = OrderStatus.Pending;

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = OrderList.Orders.Select(o=>o.Id).FirstOrDefault(),
            OrderItems = OrderList.Orders.Select(oi=>oi.OrderItems).Select(oi => new Shared.Messages.OrderItemMessage
            {
                ProductId = oi.Select(oi=>oi.Id).FirstOrDefault(),
                Count = oi.Select(oi => oi.Stock).FirstOrDefault()
            }).ToList()
        };

        await _publishEndPoint.Publish(orderCreatedEvent);

        return CreatedAtAction("create",default);
    }
}
