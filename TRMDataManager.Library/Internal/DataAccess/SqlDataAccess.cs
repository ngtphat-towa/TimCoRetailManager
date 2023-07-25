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
    internal class SqlDataAccess :  IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private bool isClosed = false;

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
        /// <summary>
        /// Starts a transaction with the given connection string name.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string.</param>
        // open connection/start transaction method
        public void StartTransaction(string connectionStringName)
        {
            // convert connection string name to actual connection string from Configuration Manager
            string connectionString = GetConnectionString(connectionStringName);
            // create connection
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        // load using the transaction
        /// <summary>
        /// Loads data in a transaction using the given stored procedure and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <typeparam name="U">The type of the parameters.</typeparam>
        /// <param name="storedProcedure">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        /// <returns>A list of data of type T.</returns>
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;
        }

        // save using the transaction
        /// <summary>
        /// Saves data in a transaction using the given stored procedure and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the parameters.</typeparam>
        /// <param name="storedProcedure">The stored procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the stored procedure.</param>
        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        // close connection/stop transaction
        /// <summary>
        /// Commits a Transaction. Call if a transaction has succeeded.
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }


        /// Rolls back a Transaction. Call if a transaction has failed.
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        //dispose
        public void Dispose()
        {
            CommitTransaction();
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    // TODO: Log this issue
                }
            }

            _transaction = null;
            _connection = null;
        }



    }


}
