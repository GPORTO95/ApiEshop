namespace Domain.Products;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(ProductId id)
        : base($"The product with ID = {id} was not found")
    { }
}
