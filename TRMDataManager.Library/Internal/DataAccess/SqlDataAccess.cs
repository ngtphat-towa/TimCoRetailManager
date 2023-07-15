using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        /// <summary>
        /// Gets the connection string with the specified name from the configuration.
        /// </summary>
        /// <param name="name">The name of the connection string.</param>
        /// <returns>The connection string.</returns>
        public string GetConnectionString(string name)
        {
            // Get the connection string with the specified name from the configuration
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Loads data from the database using the specified stored procedure and parameters.
        /// </summary>
        /// <typeparam name="T">The type of data to load.</typeparam>
        /// <typeparam name="U">The type of the parameters.</typeparam>
        /// <param name="storedProcedure">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <returns>A list of data loaded from the database.</returns>
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            // Get the connection string with the specified name
            string connectionString = GetConnectionString(connectionStringName);

            // Create a new SqlConnection with the specified connection string
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Execute the stored procedure and get the results
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                // Return the results
                return rows;
            }
        }

        /// <summary>
        /// Saves data to the database using the specified stored procedure and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <param name="storedProcedure">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            // Get the connection string with the specified name
            string connectionString = GetConnectionString(connectionStringName);

            // Create a new SqlConnection with the specified connection string
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Execute the stored procedure with the specified parameters
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }


}
