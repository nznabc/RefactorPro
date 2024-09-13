using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.DataAccess
{
    internal class DataAccessLayer : IDataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer()
        {
            _connectionString = Helpers.GetConnectionString();
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, SqlParameter[] sqlParameters, Func<SqlDataReader, T> mapFunction)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(sqlParameters);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var results = new List<T>();

                            while (await reader.ReadAsync())
                            {
                                var item = mapFunction(reader);
                                results.Add(item);
                            }

                            return results;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while quering the product.", ex);
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] sqlParameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(sqlParameters);

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while saving the product.", ex);
            }
        }
    }
}
