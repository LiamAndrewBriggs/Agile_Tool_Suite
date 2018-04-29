using System;

namespace Agile_Tool_Suite
{
    public partial class NewUser : System.Web.UI.Page
    {
        string user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            if (user != null)
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
            userLabel.Text = "Welcome! " + SQL_Helpers.getUserName(user);
        }

        protected void Search_Project(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;
            MySql.Data.MySqlClient.MySqlDataReader reader;

            errorMessage.Text = "";

            conn = SQL_Helpers.createConnection();
            conn.Open();

            string projectID = "";

            string projectInvite = searchTextBox.Text;

            string queryStr = "SELECT projectID FROM agiledb.project WHERE projectInvite=?inv";

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?inv", projectInvite);
            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                projectID = reader.GetString(reader.GetOrdinal("projectID"));
            }

            conn.Close();

            if (projectID.Equals(""))
            {
                errorMessage.Text = "Project Code not Valid";
            }
            else
            {
                conn.Open();

                queryStr = "INSERT INTO agiledb.userprojects (userID, projectID) " +
                "VALUES(?user, ?project)";

                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?user", user);
                cmd.Parameters.AddWithValue("?project", projectID);

                cmd.ExecuteReader();
                conn.Close();

                Response.BufferOutput = true;
                Response.Redirect("AccountPage.aspx", false);
            }
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