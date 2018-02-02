using System;

namespace Agile_Tool_Suite
{
    public partial class NewUser : System.Web.UI.Page
    {
        string user;
        string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            if (user == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Home.aspx", false);
            }
            else
            {
                addUserToProject();
            }
        }

        protected void addUserToProject()
        {
            MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
            conn.Open();

            string queryStr = "SELECT firstName, lastName FROM AgileDB.Users WHERE userID=?id";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?id", user);

            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                name = reader.GetString(reader.GetOrdinal("firstName")) + " " 
                    + reader.GetString(reader.GetOrdinal("lastName"));
            }

            userLabel.Text = "Welcome, " + name;
        }

        protected void Search_Project(object sender, EventArgs e)
        {

        }

        protected void Create_Project(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Create_Project.aspx", false);
        }

        protected void LogoutMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Home.aspx", false);
        }
    }
}