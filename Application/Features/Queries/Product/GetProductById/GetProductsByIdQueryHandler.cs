using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;

namespace Application.Features.Queries.Product.GetProduct
{
    public class GetProductsByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly IProductRepository _repository;

        public GetProductsByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles retrieving a product by its unique identifier
        /// </summary>
        /// <param name="request">The query containing the product ID to retrieve</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the requested <see cref="ProductDTO"/></returns>
        /// <exception cref="ProductException">
        /// Thrown when:
        /// <para>1. The provided product ID is empty (using <see cref="ProductMessageException.IdIsRequired"/>)</para>
        /// <para>2. No product is found with the specified ID (using <see cref="ProductMessageException.DataNotFound"/>)</para>
        /// </exception>
        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);

            if (product == null)
                throw new ProductException(ProductMessageException.DataNotFound);

            return product;
        }

     
    }
}
