namespace Application.Orders.GetOrder;

public class OrderResponse
{
    public OrderResponse()
    {
        
    }

    public OrderResponse(Guid id, Guid customerId, List<LineItemResponse> lineItems)
    {
        Id = id;
        CustomerId = customerId;
        LineItems = lineItems;
    }

    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public List<LineItemResponse> LineItems { get; init; } = new();
}


//public record OrderResponse(Guid Id, Guid CustomerId, List<LineItemResponse> LineItems);
