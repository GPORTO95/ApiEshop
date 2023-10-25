using Domain.Primitives;

namespace Domain.Orders;

public record OrderCreatedDomainEvent(Guid Id, OrderId Orderid) : DomainEvent(Id);
