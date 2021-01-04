using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListProductViewModel>>> Get(
            [FromServices] StoreDataContext context
        )
        {
            var products = await context.Products
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

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Product>> Get(
            [FromServices] StoreDataContext context,
            int id
        )
        {
            var product = await context.Products.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return product;
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<ResultViewModel>> Post(
            [FromServices] StoreDataContext context,
            [FromBody] EditorProductViewModel model
        )
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };
            
            var product = new Product
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                CreateDate = DateTime.Now,
                Description = model.Description,
                Image = model.Image,
                LastUpdateDate = DateTime.Now,
                Price = model.Price,
                Quantity = model.Quantity
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult<ResultViewModel>> Put(
            [FromServices] StoreDataContext context,
            [FromBody] EditorProductViewModel model
        )
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível alterar o produto",
                    Data = model.Notifications
                };
            
            var product = await context.Products.FindAsync(model.Id);
            
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            context.Entry<Product>(product).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }
    }
}