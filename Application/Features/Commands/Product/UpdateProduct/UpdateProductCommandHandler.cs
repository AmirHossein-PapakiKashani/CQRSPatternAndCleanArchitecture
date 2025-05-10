using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDTO?>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _productRepository = repository;
        }

        public async Task<ProductDTO?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Product.Id.Value, cancellationToken);
            if (product == null)
                throw new ProductException(ProductMessageException.DataNotFound);

            return await _productRepository.UpdateAsync(request.Product, cancellationToken);
        }
    }
}
