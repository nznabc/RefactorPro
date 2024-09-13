using refactor_this.Models;
using System;
using System.Threading.Tasks;

namespace refactor_this.Services
{
    /// <summary>
    /// Represents the interface for managing product options.
    /// </summary>
    public interface IProductOptionService
    {
        /// <summary>
        /// Retrieves a product option by its ID.
        /// </summary>
        /// <param name="id">The ID of the product option.</param>
        /// <returns>The product option with the specified ID.</returns>
        Task<ProductOption> GetOptionAsync(Guid id);

        /// <summary>
        /// Retrieves all product options for a given product.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>The list of product options for the specified product.</returns>
        Task<ProductOptions> GetOptionsAsync(Guid productId);

        /// <summary>
        /// Saves a new product option for a given product.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="option">The product option to save.</param>
        /// <param name="isUpdate">Indicates whether the product is being updated.</param>
        Task SaveAsync(Guid productId, ProductOption option, bool isUpdate = false);

        /// <summary>
        /// Updates an existing product option.
        /// </summary>
        /// <param name="id">The ID of the product option to update.</param>
        /// <param name="option">The updated product option.</param>
        Task UpdateAsync(Guid id, ProductOption option);

        /// <summary>
        /// Deletes a product option by its ID.
        /// </summary>
        /// <param name="id">The ID of the product option to delete.</param>
        Task DeleteAsync(Guid id);
    }
}
