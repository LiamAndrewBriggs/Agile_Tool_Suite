using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Agile_Tool_Suite
{
    public partial class LoggedIn : System.Web.UI.Page
    {
        String user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (String)(Session["uname"]);
            if (user == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Home.aspx", false);
            }
            else
            {
                MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
                conn.Open();

                String queryStr = "SELECT projectID FROM AgileDB.UserProjects WHERE userID=?id";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?id", user);

                MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows && reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        Response.BufferOutput = true;
                        Response.Redirect("NewUser.aspx", false);
                    }
                    else
                    {
                        Response.BufferOutput = true;
                        Response.Redirect("AccountPage.aspx", false);
                    }
                }

            }
        }

    }
}