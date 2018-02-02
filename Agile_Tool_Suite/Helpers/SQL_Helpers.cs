using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile_Tool_Suite
{
    public class SQL_Helpers
    {
        public static MySql.Data.MySqlClient.MySqlConnection createConnection()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connString);

            return conn;

        }
    }
}