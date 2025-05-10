using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductDTO?>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _productRepository = repository;
        }

        /// <summary>
        /// Handles the deletion of a product based on the provided command
        /// </summary>
        /// <param name="request">The command containing the product ID to delete</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the deleted <see cref="ProductDTO"/> or null if mapping fails</returns>
        /// <exception cref="ProductException">
        /// Thrown when:
        /// <para>1. The provided product ID is empty (using <see cref="ProductMessageException.IdIsRequired"/>)</para>
        /// <para>2. No product exists with the specified ID (using <see cref="ProductMessageException.DataNotFound"/>)</para>
        /// </exception>
        /// <remarks>
        /// This handler performs the following operations:
        /// <para>1. Validates the provided product ID</para>
        /// <para>2. Verifies product existence in the repository</para>
        /// <para>3. Executes the deletion operation</para>
        /// <para>4. Ensures successful deletion result</para>
        /// <para>5. Returns the deleted product DTO</para>
        /// </remarks>
        public async Task<ProductDTO?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id!.Value, cancellationToken);
            if (product == null)
                throw new ProductException(ProductMessageException.DataNotFound);

            var result = await _productRepository.DeleteAsync(request.Id.Value, cancellationToken);
           
            return result;
        }
    }
}
