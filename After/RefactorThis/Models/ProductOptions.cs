using System.Collections.Generic;

namespace refactor_this.Models
{
    /// <summary>
    /// Represents a collection of product options.
    /// </summary>
    public class ProductOptions
    {
        /// <summary>
        /// Gets the list of product options.
        /// </summary>
        public List<ProductOption> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductOptions"/> class.
        /// </summary>
        /// <param name="items">The list of product options.</param>
        public ProductOptions(List<ProductOption> items)
        {
            Items = items;
        }
    }
}