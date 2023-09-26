using NPoco.DatabaseTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess.Common
{
    public class SQLDatabase : Database
    {
        public SQLDatabase(string connectionString)
        {
            ProviderName = "System.Data.SqlClient";
            DbType = new SqlServer2012DatabaseType();
            ConnectionString = connectionString;

            EnsureSharedConnectionConfigured();
        }

        public override void EnsureSharedConnectionConfigured()
        {
            if (Connection != null) return;

            lock (_syncRoot)
            {
                Connection = new SqlConnection(ConnectionString);
                var sss = Connection.State;
                if (Connection.State == System.Data.ConnectionState.Broken)
                    Connection.Close();

                if (Connection.State == System.Data.ConnectionState.Closed)
                    Connection.Open();
            }
        }

        public override void CleanupDataBase()
        {
            /* 
             * Trying to do any clean up here fails until the Database object gets disposed.
             * The create deletes and recreates the files anyone so this isn't really necessary
             */
        }
    }
}
