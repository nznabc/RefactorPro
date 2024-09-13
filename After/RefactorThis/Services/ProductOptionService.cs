using refactor_this.DataAccess;
using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace refactor_this.Services
{
    internal class ProductOptionService : IProductOptionService
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public ProductOptionService(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<ProductOptions> GetOptionsAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            string sqlQuery = "SELECT * FROM productoption WHERE ProductId = @productId";

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@productId", productId)
                };

            List<ProductOption> items = await _dataAccessLayer.ExecuteQueryAsync<ProductOption>(sqlQuery, parameters.ToArray(), MapOption);

            return new ProductOptions(items);
        }

        public async Task<ProductOption> GetOptionAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            string sqlQuery = "SELECT * FROM ProductOption WHERE Id = @id";

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", id)
                };

            List<ProductOption> productOptions = await _dataAccessLayer.ExecuteQueryAsync<ProductOption>(sqlQuery, parameters.ToArray(), MapOption);

            return productOptions.Single();
        }

        private static ProductOption MapOption(SqlDataReader reader)
        {

            if (string.IsNullOrEmpty(reader["id"].ToString()))
            {
                return new ProductOption();
            }

            return new ProductOption(false)
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                ProductId = Guid.Parse(reader["ProductId"].ToString()),
                Name = reader["Name"].ToString(),
                Description = (DBNull.Value == reader["Description"]) ? null : reader["Description"].ToString()
            };
        }

        public async Task SaveAsync(Guid productId, ProductOption option, bool isUpdate = false)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            string sqlQuery = isUpdate ?
                "UPDATE productoption SET Name = @name, Description = @description WHERE Id = @id" :
                "INSERT INTO productoption (Id, ProductId, Name, Description) VALUES (@id, @productId, @name, @description)";

            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", option.Id),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@Name", option.Name),
                    new SqlParameter("@Description", option.Description),
                };

            await _dataAccessLayer.ExecuteNonQueryAsync(sqlQuery, parameters.ToArray());
        }

        public async Task UpdateAsync(Guid id, ProductOption option)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            var getOption = await GetOptionAsync(id);

            if (!getOption.IsNew)
                await SaveAsync(option.ProductId, option, true);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id cannot be empty");

            string sqlQuery = "DELETE FROM productoption WHERE Id = @id";

            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", id)
                };

            await _dataAccessLayer.ExecuteNonQueryAsync(sqlQuery, parameters.ToArray());
        }
    }
}