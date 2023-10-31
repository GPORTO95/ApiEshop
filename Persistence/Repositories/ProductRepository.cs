using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(ProductId id)
    {
        return await _context.Products
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }
}
