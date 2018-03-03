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

        public static String getUserName(string user)
        {
            String name = "";

            MySql.Data.MySqlClient.MySqlConnection conn = createConnection();
            conn.Open();

            string queryStr = "SELECT firstName, lastName FROM agiledb.users WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                name = reader.GetString(reader.GetOrdinal("firstName")) + " "
                    + reader.GetString(reader.GetOrdinal("lastName"));
            }

            conn.Close();

            return name;
        }

        public static String getEmail(string user)
        {
            String email = "";

            MySql.Data.MySqlClient.MySqlConnection conn = createConnection();
            conn.Open();

            string queryStr = "SELECT email FROM agiledb.users WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                email = reader.GetString(reader.GetOrdinal("email"));
            }

            conn.Close();

            return email;
        }
    }
}