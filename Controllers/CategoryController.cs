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
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromServices] StoreDataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Category>> GetCategory(
            [FromServices] StoreDataContext context,
            int id
        )
        {
            // Find() ainda não funciona com AsNoTracking
            var category = await context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return category;
        }

        [Route("{id}/products")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromServices] StoreDataContext context,
            int id
        )
        {
            var products = await context.Products.AsNoTracking().Where(x => x.Category.Id == id).ToListAsync();
            return products;
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<Category>> Post(
            [FromServices] StoreDataContext context,
            [FromBody] Category category
        )
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult<Category>> Put(
            [FromServices] StoreDataContext context,
            [FromBody] Category category
        )
        {
            if (ModelState.IsValid)
            {
                context.Entry<Category>(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("")]
        [HttpDelete]
        public async Task<ActionResult<Category>> Delete(
            [FromServices] StoreDataContext context,
            [FromBody] Category category
        )
        {
            if (ModelState.IsValid)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return category;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}