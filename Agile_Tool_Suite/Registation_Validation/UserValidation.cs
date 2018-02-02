using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile_Tool_Suite
{
    public class UserValidation
    {
        static MySql.Data.MySqlClient.MySqlConnection conn;
        static MySql.Data.MySqlClient.MySqlCommand cmd;
        static MySql.Data.MySqlClient.MySqlDataReader reader;
        static String queryStr;

        public static bool EmailCheck(string email)
        {
            bool pass = true;

            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "SELECT email FROM AgileDB.Users WHERE email=?email";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?email", email);

            reader = cmd.ExecuteReader();

            if(reader.HasRows == true)
            {
                pass = false;
            }

            return pass;
        }

        public static bool UsernameCheck(string username)
        {
            bool pass = true;

            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "SELECT username FROM AgileDB.Users WHERE username=?uname";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?uname", username);

            reader = cmd.ExecuteReader();

            if (reader.HasRows == true)
            {
                pass = false;
            }

            return pass;
        }

    }
}