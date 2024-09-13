using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace refactor_this.Models
{
    /// <summary>
    /// Represents a product option.
    /// </summary>
    public class ProductOption
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product option.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product option.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product option.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether the product option is new.
        /// </summary>
        [JsonIgnore]
        public bool IsNew { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductOption"/> class.
        /// </summary>
        /// <param name="isNew">Indicates whether the productOption is new.</param>
        public ProductOption(bool isNew)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductOption"/> class.
        /// </summary>
        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
    }
}