using refactor_this.Models;
using refactor_this.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace refactor_this.Controllers
{
    /// <summary>
    /// Controller for managing product options.
    /// </summary>
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionService _productOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductOptionsController"/> class.
        /// </summary>
        /// <param name="productOptionService">The product option service.</param>
        public ProductOptionsController(IProductOptionService productOptionService)
        {
            _productOptionService = productOptionService;
        }

        /// <summary>
        /// Gets the options for a specific product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The product options.</returns>
        [HttpGet]
        public async Task<ProductOptions> GetOptions(Guid productId)
        {
            return await _productOptionService.GetOptionsAsync(productId);
        }

        /// <summary>
        /// Gets a specific product option.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="id">The option identifier.</param>
        /// <returns>The product option.</returns>
        [HttpGet]
        public async Task<ProductOption> GetOption(Guid productId, Guid id)
        {
            var option = await _productOptionService.GetOptionAsync(id);

            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        /// <summary>
        /// Creates a new product option.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="option">The product option.</param>
        [HttpPost]
        public async Task<IHttpActionResult> CreateOption(Guid productId, ProductOption option)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productOptionService.SaveAsync(productId, option);

            return Ok();
        }

        /// <summary>
        /// Updates an existing product option.
        /// </summary>
        /// <param name="id">The option identifier.</param>
        /// <param name="option">The product option.</param>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateOption(Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productOptionService.UpdateAsync(id, option);

            return Ok();
        }

        /// <summary>
        /// Deletes a product option.
        /// </summary>
        /// <param name="id">The option identifier.</param>
        [HttpDelete]
        public async Task DeleteOption(Guid id)
        {
            await _productOptionService.DeleteAsync(id);
        }
    }
}
