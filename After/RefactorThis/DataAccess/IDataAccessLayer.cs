using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.DataAccess
{
    /// <summary>
    /// Represents the data access layer interface.
    /// </summary>
    public interface IDataAccessLayer
    {
        /// <summary>
        /// Executes a query asynchronously and maps the result to a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to be returned.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="sqlParameters">The parameters to be used in the query.</param>
        /// <param name="mapFunction">The function to map the SqlDataReader to an object of type T.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of objects.</returns>
        Task<List<T>> ExecuteQueryAsync<T>(string query, SqlParameter[] sqlParameters, Func<SqlDataReader, T> mapFunction);

        /// <summary>
        /// Executes a non-query asynchronously.
        /// </summary>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="sqlParameters">The parameters to be used in the query.</param>
        /// <returns>A task representing the asynchronous operation that returns the number of rows affected.</returns>
        Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] sqlParameters);
    }
}
