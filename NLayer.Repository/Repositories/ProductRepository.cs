using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Repository;

namespace NLayer.Repository.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Product>> GetProductsWithCategory()
    {
        //Eager Loading
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }
}