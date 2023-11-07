using Application.Data;
using Domain.Orders;
using IntegrationEvents;
using MediatR;
using Rebus.Bus;

namespace Application.Orders.Create;

internal sealed class OrderCreatedDomainEventHandler
    : INotificationHandler<OrderCreatedDomainEvent>
{
    #region :: metodo saga :: 
    //private readonly IBus _bus;

    //public OrderCreatedDomainEventHandler(IBus bus)
    //{
    //    _bus = bus;
    //}

    //public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    //{
    //    await _bus.Send(new OrderCreatedIntegrationEvent(notification.Orderid.Value));
    //}
    #endregion

    private readonly ICalculateOrderSummary _calculateOrderSummary;
    private readonly IOrderSummaryRepository _orderSummaryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedDomainEventHandler(
        ICalculateOrderSummary calculateOrderSummary,
        IOrderSummaryRepository orderSummaryRepository,
        IUnitOfWork unitOfWork)
    {
        _calculateOrderSummary = calculateOrderSummary;
        _orderSummaryRepository = orderSummaryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderSummary = await _calculateOrderSummary.CalculateAsync(notification.Orderid);

        if (orderSummary is null)
        {
            return;
        }

        _orderSummaryRepository.Add(orderSummary);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
