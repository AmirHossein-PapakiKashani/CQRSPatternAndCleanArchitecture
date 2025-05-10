using Application.Features.Commands.Product.CreateProduct;
using Application.Features.Commands.Product.DeleteProduct;
using Application.Features.Commands.Product.UpdateProduct;
using Application.Features.Queries.Product.GetAllProducts;
using Application.Features.Queries.Product.GetProduct;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SampleProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id">Product unique identifier</param>
        /// <returns>Product if founded and if not return exception</returns>
        [HttpGet]
        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            return await _mediator.Send(new GetProductByIdQuery(id));
        }

        /// <summary>
        /// Get all of products
        /// </summary>
        [HttpGet("/GetAll")]
        public async Task<List<ProductDTO>?> GetAllAsync()
        {
            return await _mediator.Send(new GetAllProductQuery());
        }

        /// <summary>
        /// Update product details 
        /// </summary>
        /// <param name="product">Product unique identifier</param>
        /// <returns>Product that updated</returns>
        [HttpPut]
        public async Task<ProductDTO?> UpdateAsync(ProductDTO product)
        {
            return await _mediator.Send(new UpdateProductCommand(product));
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id">Product unique identifier</param>
        /// <returns>Product that deleted</returns>
        [HttpDelete]
        public async Task<ProductDTO?> DeleteAsync(int id)
        {
            return await _mediator.Send(new DeleteProductCommand(id));
        }

        /// <summary>
        /// Create new product 
        /// </summary>
        /// <param name="product">Product Detail</param>
        /// <returns>Product that saved</returns>
        [HttpPost]
        public async Task<ProductDTO> AddAsync(ProductDTO product)
        {
            return await _mediator.Send(new CreateProductCommand(product));
        }
    }
}
