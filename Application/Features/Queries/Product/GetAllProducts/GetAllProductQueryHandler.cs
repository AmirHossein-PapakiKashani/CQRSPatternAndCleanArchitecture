using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductDTO?>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Handles the retrieval of all products
        /// </summary>
        /// <param name="request">The query request to get all products</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a list of nullable <see cref="ProductDTO"/> objects</returns>
        /// <exception cref="ProductException">
        /// Thrown when no products are found in the repository (using <see cref="ProductMessageException.DataNotFound"/>)
        /// </exception>
        public async Task<List<ProductDTO?>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products == null ? throw new ProductException(ProductMessageException.DataNotFound) : products;
        }
    }
}
