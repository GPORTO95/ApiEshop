using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class ProductRepository : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
