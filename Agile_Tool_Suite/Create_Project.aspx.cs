using System;

namespace Agile_Tool_Suite
{
    public partial class Create_Project : System.Web.UI.Page
    {
        string user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (string)(Session["uname"]);
            if (user == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Home.aspx", false);
            }
        }

        protected void CreateProject(object sender, EventArgs e)
        {
            string name = ProjectName.Text;
            string option = DdlMonths.SelectedValue;

            if (option.Equals("-1"))
            {
                DropDownError.Visible = true;
            }
            else
            {
                MySql.Data.MySqlClient.MySqlConnection conn = SQL_Helpers.createConnection();
                conn.Open();
                
                string queryStr = "INSERT INTO agiledb.project(projectName, primaryMethodology, projectCreator) VALUES(?name, ?method, ?userName)";

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?name", name);
                cmd.Parameters.AddWithValue("?userName", user);

                if (option.Equals("1"))
                {
                    cmd.Parameters.AddWithValue("?method", "Scrum");
                }
                else if (option.Equals("2"))
                {
                    cmd.Parameters.AddWithValue("?method", "Kanban");
                }

                MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();

                conn.Close();

                if (option.Equals("1"))
                {
                    string projectID = " ";
                    string backlogID = " ";

                    conn.Open();

                    queryStr = "SELECT projectID FROM agiledb.project ORDER BY projectID DESC LIMIT 1";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        projectID = reader.GetString(reader.GetOrdinal("projectID"));
                    }

                    conn.Close();
                    conn.Open();

                    queryStr = "INSERT INTO agiledb.userprojects(userID, projectID) VALUES(?user, ?project)";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?project", projectID);
                    cmd.Parameters.AddWithValue("?user", user);
                    reader = cmd.ExecuteReader();

                    conn.Close();
                    conn.Open();

                    queryStr = "INSERT INTO agiledb.backlog(backlogtest) VALUES(?test)";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?test", "test");

                    reader = cmd.ExecuteReader();

                    conn.Close();
                    conn.Open();

                    queryStr = "SELECT backlogID FROM agiledb.backlog ORDER BY backlogID DESC LIMIT 1";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows && reader.Read())
                    {
                        backlogID = reader.GetString(reader.GetOrdinal("backlogID"));
                    }

                    conn.Close();
                    conn.Open();

                    queryStr = "UPDATE agiledb.project SET backlogID = ?backlog WHERE projectID = ?project";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                    cmd.Parameters.AddWithValue("?backlog", backlogID);
                    cmd.Parameters.AddWithValue("?project", projectID);

                    reader = cmd.ExecuteReader();

                    conn.Close();

                    Response.BufferOutput = true;
                    Response.Redirect("AccountPage.aspx", false);
                }
                else
                {
                    conn.Close();
                    Response.BufferOutput = true;
                    Response.Redirect("AccountPage.aspx", false);
                }
            }
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