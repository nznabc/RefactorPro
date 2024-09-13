using System.Collections.Generic;

namespace refactor_this.Models
{
    /// <summary>
    /// Represents a collection of products.
    /// </summary>
    public class Products
    {
        /// <summary>
        /// Gets the list of items in the collection.
        /// </summary>
        public List<Product> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Products"/> class.
        /// </summary>
        /// <param name="items">The list of items to initialize the collection with.</param>
        public Products(List<Product> items)
        {
            Items = items;
        }
    }
}