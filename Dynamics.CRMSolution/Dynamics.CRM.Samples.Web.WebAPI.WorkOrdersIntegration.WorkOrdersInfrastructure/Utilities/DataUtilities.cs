using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Arcos.CUC.WorkOrdersInfrastructure
{
    public class DataUtilities
    {
        public static string GetConnectionString()
        {
            string connectionString = Properties.Settings.Default.companyDatabaseConnection;
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(connectionString);
            return connection.ToString();
        }

        public static string GetProvider()
        {
            string name = Properties.Settings.Default.providerName;
            return name;
        }
    }
}
