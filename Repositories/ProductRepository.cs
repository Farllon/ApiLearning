using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Repositories
{
    public class ProductRepository
    {
        private StoreDataContext _context;

        public ProductRepository(StoreDataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ListProductViewModel>>> Get()
        {
            var products = await _context.Products
                .Include(x => x.Category)
                .Select(x => new ListProductViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Category = x.Category.Title,
                    CategoryId = x.CategoryId
                })
                .AsNoTracking()
                .ToListAsync();

            return products;
        }

        public async Task<ActionResult<Product>> Get(int id)
        {
            // var product = await _context
            //     .Products
            //     .AsNoTracking()
            //     .Where(x => x.Id == id)
            //     .FirstOrDefaultAsync();

            var product = await _context.Products.FindAsync(id);

            return product;
        }

        public async Task Save(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            _context.Entry<Product>(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}