using refactor_this.Models;
using System;
using System.Threading.Tasks;

namespace refactor_this.Services
{
    /// <summary>
    /// Represents a service for managing products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>The list of products.</returns>
        Task<Products> GetAllAsync();

        /// <summary>
        /// Searches products by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The list of products matching the name.</returns>
        Task<Products> SearchByNameAsync(string name);

        /// <summary>
        /// Gets a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        Task<Product> GetProductAsync(Guid id);

        /// <summary>
        /// Saves a new product.
        /// </summary>
        /// <param name="product">The product to save.</param>
        /// <param name="isUpdate">Indicates whether the product is being updated.</param>
        Task SaveAsync(Product product, bool isUpdate = false);

        /// <summary>
        /// Updates a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product.</param>
        Task UpdateAsync(Guid id, Product product);

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        Task DeleteAsync(Guid id);
    }
}
