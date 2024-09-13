using refactor_this.Models;
using refactor_this.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace refactor_this.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All products.</returns>
        [HttpGet]
        public async Task<Products> GetAll()
        {
            return await _productService.GetAllAsync();
        }

        /// <summary>
        /// Searches products by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>Products matching the name.</returns>
        [HttpGet]
        public async Task<Products> SearchByName(string name)
        {
            return await _productService.SearchByNameAsync(name);
        }

        /// <summary>
        /// Gets a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        [HttpGet]
        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        [HttpPost]
        public async Task<IHttpActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.SaveAsync(product);

            return Ok();
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product.</param>
        [HttpPut]
        public async Task<IHttpActionResult> Update(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.UpdateAsync(id, product);

            return Ok();

        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
        }
    }
}
