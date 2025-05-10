using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Adds a new product to the repository using the provided DTO
        /// </summary>
        /// <param name="item">The product data transfer object containing product information</param>
        /// <param name="cancellation">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the created <see cref="ProductDTO"/> or null if mapping fails</returns>
        Task<ProductDTO?> AddAsync(ProductDTO item, CancellationToken cancellation);

        /// <summary>
        /// Deletes a product by its unique identifier
        /// </summary>
        /// <param name="id">The GUID of the product to delete</param>
        /// <param name="cancellation">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the deleted <see cref="ProductDTO"/> or null if mapping fails</returns>
        /// <exception cref="ProductException">
        /// Thrown when:
        /// <para>1. No product is found with the specified ID (using <see cref="ProductMessageException.DataNotFound"/>)</para>
        /// <para>2. The entity could not be properly mapped to DTO</para>
        /// </exception>
        Task<List<ProductDTO?>> GetAllAsync(CancellationToken cancellation);

        /// <summary>
        /// Retrieves a product by its unique identifier from the repository
        /// </summary>
        /// <param name="id">The GUID of the product to retrieve</param>
        /// <param name="cancellation">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the found <see cref="ProductDTO"/> or null if no product exists with the specified ID</returns>
        Task<ProductDTO?> GetByIdAsync(int id, CancellationToken cancellation);

        /// <summary>
        /// Updates an existing product in the repository using the provided DTO
        /// </summary>
        /// <param name="item">The product data transfer object containing updated information</param>
        /// <param name="cancellation">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the updated <see cref="ProductDTO"/> or null if mapping fails</returns>
        Task<ProductDTO?> UpdateAsync(ProductDTO item, CancellationToken cancellation);

        /// <summary>
        /// Deletes a product from the repository by its unique identifier
        /// </summary>
        /// <param name="id">The GUID of the product to delete</param>
        /// <param name="cancellation">A token to monitor for cancellation requests</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the deleted <see cref="ProductDTO"/> or null if mapping fails</returns>
        Task<ProductDTO?> DeleteAsync(int id, CancellationToken cancellation);
    }
}
