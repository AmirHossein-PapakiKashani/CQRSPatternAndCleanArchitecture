using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDTO>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Handles the creation of a new product based on the provided command.
        /// </summary>
        /// <param name="request">The command containing the product data to create.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{ProductDTO}"/> representing the result of the operation, containing the created product DTO.</returns>
        /// <exception cref="ProductException">Thrown when the product creation fails (returns null).</exception>
        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Product.Id!.Value, cancellationToken);
            if (product != null)
                throw new ProductException(ProductMessageException.ProductAlreadyExist);

            var result = await _productRepository.AddAsync(request.Product, cancellationToken);

            if (result == null)
                throw new ProductException(ProductMessageException.CreateFailed);

            return result;
        }
    }
}
