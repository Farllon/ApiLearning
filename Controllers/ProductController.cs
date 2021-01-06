using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        private ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListProductViewModel>>> Get()
        {
            return await _repository.Get();
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Product>> Get(int id)
        {
            return await _repository.Get(id);
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult<ResultViewModel>> Post([FromBody] EditorProductViewModel model)
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

            await _repository.Save(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult<ResultViewModel>> Put([FromBody] EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível alterar o produto",
                    Data = model.Notifications
                };
            
            var product = await _repository.Get(model.Id);
            
            product.Value.Title = model.Title;
            product.Value.CategoryId = model.CategoryId;
            product.Value.Description = model.Description;
            product.Value.Image = model.Image;
            product.Value.LastUpdateDate = DateTime.Now;
            product.Value.Price = model.Price;
            product.Value.Quantity = model.Quantity;

            await _repository.Update(product.Value);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }
    }
}