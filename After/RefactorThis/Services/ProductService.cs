using refactor_this.DataAccess;
using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace refactor_this.Services
{

    internal class ProductService : IProductService
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly IProductOptionService _productOptionService;

        public ProductService(IDataAccessLayer dataAccessLayer, IProductOptionService productOptionService)
        {
            _dataAccessLayer = dataAccessLayer;
            _productOptionService = productOptionService;
        }

        public Task<Products> GetAllAsync()
        {
            return LoadProductsAsync(null);
        }

        public Task<Products> SearchByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "name cannot be null or empty");

            return LoadProductsAsync(name.ToLower());
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "id cannot be empty");

            var sqlQuery = "SELECT * FROM product WHERE Id = @id";

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", id)
                };

            List<Product> products = await _dataAccessLayer.ExecuteQueryAsync<Product>(sqlQuery, parameters.ToArray(), MapProduct);

            return products.Single();
        }

        private async Task<Products> LoadProductsAsync(string where)
        {
            var sqlQuery = "SELECT * FROM Product";

            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(where))
            {
                sqlQuery += " WHERE Name LIKE @where";

                parameters.Add(new SqlParameter("@where", "%" + where + "%"));
            }

            var items = await _dataAccessLayer.ExecuteQueryAsync<Product>(sqlQuery, parameters.ToArray(), MapProduct);

            return new Products(items);
        }

        private static Product MapProduct(SqlDataReader reader)
        {
            if (string.IsNullOrEmpty(reader["Id"].ToString()))
            {
                return new Product();
            }

            return new Product(false)
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                Description = (DBNull.Value == reader["Description"]) ? null : reader["Description"].ToString(),
                Price = decimal.Parse(reader["Price"].ToString()),
                DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString())
            };
        }

        public async Task SaveAsync(Product product, bool isUpdate = false)
        {
            var sqlQuery = isUpdate ?
                "UPDATE Product SET Name = @name, Description = @description, Price = @price, DeliveryPrice = @deliveryprice WHERE Id = @id" :
                "INSERT INTO Product (Id, Name, Description, Price, DeliveryPrice) VALUES (@id, @name, @description, @price, @deliveryprice)";

            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", product.Id),
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Description", product.Description),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@DeliveryPrice", product.DeliveryPrice)
                };

            await _dataAccessLayer.ExecuteNonQueryAsync(sqlQuery, parameters.ToArray());
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "id cannot be empty");

            var getProduct = await GetProductAsync(id);

            if (!getProduct.IsNew)
                await SaveAsync(product, true);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "id cannot be empty");

            var options = await _productOptionService.GetOptionsAsync(id);

            foreach (var option in options.Items)
            {
                await _productOptionService.DeleteAsync(option.Id);
            }

            var sqlQuery = "DELETE FROM Product WHERE Id = @Id";

            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", id)
                };

            await _dataAccessLayer.ExecuteNonQueryAsync(sqlQuery, parameters.ToArray());
        }
    }
}