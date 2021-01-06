using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        private StoreDataContext _context;

        public CategoryController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            // Find() ainda não funciona com AsNoTracking
            var category = await _context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return category;
        }

        [Route("{id}/products")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int id)
        {
            var products = await _context.Products.AsNoTracking().Where(x => x.Category.Id == id).ToListAsync();
            return products;
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult<Category>> Put([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Entry<Category>(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("")]
        [HttpDelete]
        public async Task<ActionResult<Category>> Delete([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}