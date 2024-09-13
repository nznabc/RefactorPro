using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace refactor_this.Models
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Range(0, 20000)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the delivery price of the product.
        /// </summary>
        [Range(0, 1000)]
        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is new.
        /// </summary>
        [JsonIgnore]
        public bool IsNew { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="isNew">Indicates whether the product is new.</param>
        public Product(bool isNew)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
    }
}