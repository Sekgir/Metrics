using MetricsManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
    public class SQLiteConnectionManager : IConnectionManager
    {
        private readonly string _connectionString;
        public SQLiteConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateOpenedConnection()
        {
            IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
